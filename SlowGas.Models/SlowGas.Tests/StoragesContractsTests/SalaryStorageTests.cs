using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SlowGas.Database;
using SlowGas.Database.Implementations;
using SlowGas.Models.DataModels;

namespace SlowGas.Tests.StoragesContractsTests
{
    [TestFixture]
    public class SalaryStorageTests
    {
        private SalaryStorage _salaryStorage;
        private SlowGasDbContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<SlowGasDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SlowGasDbContext(options);
            _salaryStorage = new SalaryStorage(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void AddElement_ShouldSaveSalary()
        {
            var salary = new SalaryDataModel(
                Guid.NewGuid().ToString(),
                DateTime.Now,
                50000
            );

            _salaryStorage.AddElement(salary);

            var saved = _context.Salaries.FirstOrDefault(s => s.SalaryId == salary.Id);
            Assert.That(saved, Is.Not.Null);
            Assert.That(saved.Amount, Is.EqualTo(50000));
        }
    }
}