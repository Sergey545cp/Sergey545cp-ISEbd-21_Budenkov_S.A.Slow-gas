using Moq;
using NUnit.Framework;
using SlowGas.BusinessLogic.Implementations;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Infrastructure;
using SlowGas.Models.Infrastructure.PostConfigurations;
using SlowGas.Models.StoragesContracts;
using Microsoft.Extensions.Logging;

namespace SlowGas.Tests.BusinessLogicsContractsTests
{
    [TestFixture]
    public class SalaryBusinessLogicTests
    {
        private SalaryBusinessLogic _salaryBusinessLogic;
        private Mock<ISalaryStorageContract> _salaryStorageMock;
        private Mock<ISaleStorageContract> _saleStorageMock;
        private Mock<IPostStorageContract> _postStorageMock;
        private Mock<IWorkerStorageContract> _workerStorageMock;
        private Mock<IConfigurationSalary> _configSalaryMock;
        private Mock<ILogger<SalaryBusinessLogic>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _salaryStorageMock = new Mock<ISalaryStorageContract>();
            _saleStorageMock = new Mock<ISaleStorageContract>();
            _postStorageMock = new Mock<IPostStorageContract>();
            _workerStorageMock = new Mock<IWorkerStorageContract>();
            _configSalaryMock = new Mock<IConfigurationSalary>();
            _loggerMock = new Mock<ILogger<SalaryBusinessLogic>>();

            _configSalaryMock.Setup(x => x.ExtraSaleSum).Returns(10000);

            _salaryBusinessLogic = new SalaryBusinessLogic(
                _salaryStorageMock.Object,
                _saleStorageMock.Object,
                _postStorageMock.Object,
                _workerStorageMock.Object,
                _configSalaryMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public void GetAllSalariesByPeriod_ShouldThrowException_WhenDatesIncorrect()
        {
            var fromDate = DateTime.Now;
            var toDate = DateTime.Now.AddDays(-1);

            Assert.That(() => _salaryBusinessLogic.GetAllSalariesByPeriod(fromDate, toDate),
                Throws.TypeOf<IncorrectDatesException>());
        }

        [Test]
        public void GetAllSalariesByPeriod_ShouldReturnList_WhenDatesCorrect()
        {
            var fromDate = DateTime.Now.AddDays(-30);
            var toDate = DateTime.Now;
            var expectedList = new List<SalaryDataModel>();

            _salaryStorageMock.Setup(x => x.GetList(fromDate, toDate)).Returns(expectedList);

            var result = _salaryBusinessLogic.GetAllSalariesByPeriod(fromDate, toDate);

            Assert.That(result, Is.EqualTo(expectedList));
        }

        [Test]
        public void GetAllSalariesByPeriodByWorker_ShouldThrowException_WhenWorkerIdEmpty()
        {
            var fromDate = DateTime.Now.AddDays(-30);
            var toDate = DateTime.Now;

            // ИСПРАВЛЕНО: ожидаем ArgumentNullException
            Assert.That(() => _salaryBusinessLogic.GetAllSalariesByPeriodByWorker(fromDate, toDate, ""),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GetAllSalariesByPeriodByWorker_ShouldThrowException_WhenWorkerIdInvalid()
        {
            var fromDate = DateTime.Now.AddDays(-30);
            var toDate = DateTime.Now;

            Assert.That(() => _salaryBusinessLogic.GetAllSalariesByPeriodByWorker(fromDate, toDate, "not-a-guid"),
                Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void CalculateSalaryByMonth_WithCashierConfig_ShouldCalculateCorrectly()
        {
            var workerId = Guid.NewGuid().ToString();
            var postId = Guid.NewGuid().ToString();

            var worker = new WorkerDataModel(workerId, "Тест", postId, DateTime.Now.AddYears(-20), DateTime.Now, false);
            var post = new PostDataModel(postId, "Кассир", PostType.CashierConsultant,
                new CashierPostConfiguration { Rate = 10000, SalePercent = 0.05, BonusForExtraSales = 0.1 });

            var product = new SaleProductDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 2, 6000);
            var sales = new List<SaleDataModel>
            {
                new SaleDataModel(Guid.NewGuid().ToString(), workerId, null, DiscountType.None, false, new List<SaleProductDataModel> { product })
            };

            _workerStorageMock.Setup(x => x.GetList()).Returns(new List<WorkerDataModel> { worker });
            _postStorageMock.Setup(x => x.GetElementById(postId)).Returns(post);
            _saleStorageMock.Setup(x => x.GetList(It.IsAny<DateTime>(), It.IsAny<DateTime>(), workerId, null, false))
                .Returns(sales);

            SalaryDataModel savedSalary = null;
            _salaryStorageMock.Setup(x => x.AddElement(It.IsAny<SalaryDataModel>()))
                .Callback<SalaryDataModel>(x => savedSalary = x);

            _salaryBusinessLogic.CalculateSalaryByMonth(DateTime.Now);

            Assert.That(savedSalary, Is.Not.Null);
            Assert.That(savedSalary.Salary, Is.GreaterThan(10000));
        }

        [Test]
        public void CalculateSalaryByMonth_WithSupervisorConfig_ShouldCalculateCorrectly()
        {
            var workerId = Guid.NewGuid().ToString();
            var postId = Guid.NewGuid().ToString();

            var worker = new WorkerDataModel(workerId, "Тест", postId, DateTime.Now.AddYears(-20), DateTime.Now, false);
            var post = new PostDataModel(postId, "Руководитель", PostType.Supervisor,
                new SupervisorPostConfiguration { Rate = 15000, PersonalCountTrendPremium = 500 });

            _workerStorageMock.Setup(x => x.GetList()).Returns(new List<WorkerDataModel> { worker });
            _postStorageMock.Setup(x => x.GetElementById(postId)).Returns(post);
            _saleStorageMock.Setup(x => x.GetList(It.IsAny<DateTime>(), It.IsAny<DateTime>(), workerId, null, false))
                .Returns(new List<SaleDataModel>());
            _workerStorageMock.Setup(x => x.GetWorkerTrend(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(3);

            SalaryDataModel savedSalary = null;
            _salaryStorageMock.Setup(x => x.AddElement(It.IsAny<SalaryDataModel>()))
                .Callback<SalaryDataModel>(x => savedSalary = x);

            _salaryBusinessLogic.CalculateSalaryByMonth(DateTime.Now);

            Assert.That(savedSalary, Is.Not.Null);
            Assert.That(savedSalary.Salary, Is.EqualTo(15000 + 3 * 500));
        }

        [Test]
        public void CalculateSalaryByMonth_WithBasicConfig_ShouldCalculateCorrectly()
        {
            var workerId = Guid.NewGuid().ToString();
            var postId = Guid.NewGuid().ToString();

            var worker = new WorkerDataModel(workerId, "Тест", postId, DateTime.Now.AddYears(-20), DateTime.Now, false);
            var post = new PostDataModel(postId, "Сотрудник", PostType.Assistant,
                new PostConfiguration { Rate = 20000 });

            _workerStorageMock.Setup(x => x.GetList()).Returns(new List<WorkerDataModel> { worker });
            _postStorageMock.Setup(x => x.GetElementById(postId)).Returns(post);
            _saleStorageMock.Setup(x => x.GetList(It.IsAny<DateTime>(), It.IsAny<DateTime>(), workerId, null, false))
                .Returns(new List<SaleDataModel>());

            SalaryDataModel savedSalary = null;
            _salaryStorageMock.Setup(x => x.AddElement(It.IsAny<SalaryDataModel>()))
                .Callback<SalaryDataModel>(x => savedSalary = x);

            _salaryBusinessLogic.CalculateSalaryByMonth(DateTime.Now);

            Assert.That(savedSalary, Is.Not.Null);
            Assert.That(savedSalary.Salary, Is.EqualTo(20000));
        }
    }
}