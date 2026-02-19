using NUnit.Framework;
using SlowGas.Models.DataModels;
using SlowGas.Models.Exceptions;

namespace SlowGas.Tests.DataModelsTests
{
    [TestFixture]
    public class CustomerTests
    {
        [Test]
        public void Customer_ValidData_ShouldPass()
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Иванов Иван",
                Phone = "+79161234567",
                Email = "ivan@mail.ru"
            };

            Assert.That(() => customer.Validate(), Throws.Nothing);
        }

        [Test]
        public void Customer_EmptyId_ShouldThrowException()
        {
            var customer = new Customer
            {
                Id = "",
                Name = "Иванов Иван",
                Phone = "+79161234567",
                Email = "ivan@mail.ru"
            };

            Assert.That(() => customer.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Customer_EmptyName_ShouldThrowException()
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "",
                Phone = "+79161234567",
                Email = "ivan@mail.ru"
            };

            Assert.That(() => customer.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Customer_InvalidPhone_ShouldThrowException()
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Иванов Иван",
                Phone = "12345",
                Email = "ivan@mail.ru"
            };

            Assert.That(() => customer.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Customer_EmptyPhone_ShouldThrowException()
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Иванов Иван",
                Phone = "",
                Email = "ivan@mail.ru"
            };

            Assert.That(() => customer.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Customer_EmptyEmail_ShouldThrowException()
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Иванов Иван",
                Phone = "+79161234567",
                Email = ""
            };

            Assert.That(() => customer.Validate(), Throws.TypeOf<ValidationException>());
        }
    }
}