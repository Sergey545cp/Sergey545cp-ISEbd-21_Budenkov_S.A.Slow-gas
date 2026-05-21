using SlowGas.Models.DataModels;

namespace SlowGas.Models.BusinessLogicsContracts
{
    public interface ISalaryBusinessLogic
    {
        List<SalaryDataModel> GetAllSalariesByPeriod(DateTime fromDate, DateTime toDate);
        List<SalaryDataModel> GetAllSalariesByPeriodByWorker(DateTime fromDate, DateTime toDate, string workerId);
        void CalculateSalaryByMonth(DateTime date);
    }
}