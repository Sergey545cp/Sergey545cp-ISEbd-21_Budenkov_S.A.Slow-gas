using SlowGas.Models.BusinessLogicsContracts;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using SlowGas.Models.Infrastructure;
using SlowGas.Models.Infrastructure.PostConfigurations;
using SlowGas.Models.StoragesContracts;
using Microsoft.Extensions.Logging;

namespace SlowGas.BusinessLogic.Implementations
{
    public class SalaryBusinessLogic : ISalaryBusinessLogic
    {
        private readonly ISalaryStorageContract _salaryStorage;
        private readonly ISaleStorageContract _saleStorage;
        private readonly IPostStorageContract _postStorage;
        private readonly IWorkerStorageContract _workerStorage;
        private readonly IConfigurationSalary _salaryConfiguration;
        private readonly ILogger<SalaryBusinessLogic> _logger;
        private static readonly object _lockObject = new object();

        public SalaryBusinessLogic(
            ISalaryStorageContract salaryStorage,
            ISaleStorageContract saleStorage,
            IPostStorageContract postStorage,
            IWorkerStorageContract workerStorage,
            IConfigurationSalary salaryConfiguration,
            ILogger<SalaryBusinessLogic> logger)
        {
            _salaryStorage = salaryStorage;
            _saleStorage = saleStorage;
            _postStorage = postStorage;
            _workerStorage = workerStorage;
            _salaryConfiguration = salaryConfiguration;
            _logger = logger;
        }

        public List<SalaryDataModel> GetAllSalariesByPeriod(DateTime fromDate, DateTime toDate)
        {
            if (fromDate >= toDate)
                throw new IncorrectDatesException(fromDate, toDate);

            var result = _salaryStorage.GetList(fromDate, toDate);
            if (result == null)
                throw new NullListException();
            return result;
        }

        public List<SalaryDataModel> GetAllSalariesByPeriodByWorker(DateTime fromDate, DateTime toDate, string workerId)
        {
            if (fromDate >= toDate)
                throw new IncorrectDatesException(fromDate, toDate);
            if (string.IsNullOrWhiteSpace(workerId))
                throw new ArgumentNullException(nameof(workerId));
            if (!workerId.IsGuid())
                throw new ValidationException("WorkerId is not a valid GUID");

            var result = _salaryStorage.GetList(fromDate, toDate, workerId);
            if (result == null)
                throw new NullListException();
            return result;
        }

        public void CalculateSalaryByMonth(DateTime date)
        {
            _logger.LogInformation("CalculateSalaryByMonth: {date}", date);

            var startDate = new DateTime(date.Year, date.Month, 1);
            var finishDate = new DateTime(date.Year, date.Month,
                DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);

            var workers = _workerStorage.GetList() ?? throw new NullListException();

            Parallel.ForEach(workers, worker =>
            {
                try
                {
                    CalculateSalaryForWorker(worker, startDate, finishDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error calculating salary for worker {WorkerId}", worker.Id);
                }
            });
        }

        private void CalculateSalaryForWorker(WorkerDataModel worker, DateTime startDate, DateTime finishDate)
        {
            var sales = _saleStorage.GetList(startDate, finishDate, workerId: worker.Id, customerId: null, isReturned: false) ?? new List<SaleDataModel>();
            var post = _postStorage.GetElementById(worker.PostId) ?? throw new NullListException();

            double salary = 0;

            switch (post.ConfigurationModel)
            {
                case CashierPostConfiguration config:
                    salary = CalculateSalaryForCashier(sales, startDate, finishDate, config);
                    break;
                case SupervisorPostConfiguration config:
                    salary = CalculateSalaryForSupervisor(startDate, finishDate, config);
                    break;
                case PostConfiguration config:
                    salary = config.Rate;
                    break;
                default:
                    salary = post.ConfigurationModel?.Rate ?? 0;
                    break;
            }

            _logger.LogDebug("Employee {WorkerId} salary: {salary}", worker.Id, salary);

            var salaryRecord = new SalaryDataModel(worker.Id, finishDate, salary);
            _salaryStorage.AddElement(salaryRecord);
        }

        private double CalculateSalaryForCashier(List<SaleDataModel> sales, DateTime startDate, DateTime finishDate, CashierPostConfiguration config)
        {
            var tasks = new List<Task>();
            var calcPercent = 0.0;

            for (var date = startDate; date <= finishDate; date = date.AddDays(1))
            {
                var currentDate = date;
                tasks.Add(Task.Run(() =>
                {
                    var salesInDay = sales.Where(x => x.SaleDate.Date == currentDate.Date).ToList();
                    if (salesInDay.Count > 0)
                    {
                        var avgSale = salesInDay.Average(x => x.Sum);
                        var percentForDay = avgSale * config.SalePercent;

                        lock (_lockObject)
                        {
                            calcPercent += percentForDay;
                        }
                    }
                }));
            }

            var bonusTask = Task.Run(() =>
            {
                return sales.Where(x => x.Sum > _salaryConfiguration.ExtraSaleSum).Sum(x => x.Sum) * config.BonusForExtraSales;
            });

            try
            {
                Task.WaitAll([.. tasks, bonusTask]);
            }
            catch (AggregateException ex)
            {
                foreach (var innerEx in ex.InnerExceptions)
                {
                    _logger.LogError(innerEx, "Error in cashier salary calculation");
                }
                return config.Rate;
            }

            return config.Rate + calcPercent + bonusTask.Result;
        }

        private double CalculateSalaryForSupervisor(DateTime startDate, DateTime finishDate, SupervisorPostConfiguration config)
        {
            var trend = _workerStorage.GetWorkerTrend(startDate, finishDate);
            return config.Rate + config.PersonalCountTrendPremium * trend;
        }
    }
}