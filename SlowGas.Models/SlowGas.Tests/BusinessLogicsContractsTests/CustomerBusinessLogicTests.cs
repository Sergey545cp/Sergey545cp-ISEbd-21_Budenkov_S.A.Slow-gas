using Moq;
using SlowGas.BusinessLogic.Implementations;
using SlowGas.Models.DataModels;
using SlowGas.Models.Exceptions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SlowGas.Contracts.StoragesContracts;

namespace SlowGas.Tests.BusinessLogicsContractsTests
{
    [TestFixture]
    public class CustomerBusinessLogicTests
    {
        private CustomerBusinessLogic _customerBusinessLogic;
        private Mock<ICustomerStorageContract> _customerStorageMock;
        private Mock<ILogger> _loggerMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _customerStorageMock = new Mock<ICustomerStorageContract>();
            _loggerMock = new Mock<ILogger>();
            _customerBusinessLogic = new CustomerBusinessLogic(_customerStorageMock.Object, _loggerMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _customerStorageMock.Reset();
        }

        [Test]
        public void GetAllCustomers_ReturnsList_WhenStorageHasData()
        {
            var expected = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid().ToString(), Name = "Клиент 1", Phone = "+79161234567", Email = "test1@mail.ru" },
                new Customer { Id = Guid.NewGuid().ToString(), Name = "Клиент 2", Phone = "+79161234568", Email = "test2@mail.ru" }
            };
            _customerStorageMock.Setup(x => x.GetList()).Returns(expected);

            var result = _customerBusinessLogic.GetAllCustomers();

            Assert.That(result, Is.EqualTo(expected));
            _customerStorageMock.Verify(x => x.GetList(), Times.Once);
        }

        [Test]
        public void GetAllCustomers_ThrowsNullListException_WhenStorageReturnsNull()
        {
            _customerStorageMock.Setup(x => x.GetList()).Returns((List<Customer>)null);
            Assert.That(() => _customerBusinessLogic.GetAllCustomers(), Throws.TypeOf<NullListException>());
        }

        [Test]
        public void GetCustomerByData_ById_ReturnsCustomer()
        {
            var id = Guid.NewGuid().ToString();
            var expected = new Customer { Id = id, Name = "Тест", Phone = "+79161234567", Email = "test@mail.ru" };
            _customerStorageMock.Setup(x => x.GetElementById(id)).Returns(expected);

            var result = _customerBusinessLogic.GetCustomerByData(id);

            Assert.That(result, Is.EqualTo(expected));
            _customerStorageMock.Verify(x => x.GetElementById(id), Times.Once);
        }

        [Test]
        public void GetCustomerByData_ByName_ReturnsCustomer()
        {
            var name = "ООО Ромашка";
            var expected = new Customer { Id = Guid.NewGuid().ToString(), Name = name, Phone = "+79161234567", Email = "test@mail.ru" };
            _customerStorageMock.Setup(x => x.GetElementByName(name)).Returns(expected);

            var result = _customerBusinessLogic.GetCustomerByData(name);

            Assert.That(result, Is.EqualTo(expected));
            _customerStorageMock.Verify(x => x.GetElementByName(name), Times.Once);
        }

        [Test]
        public void GetCustomerByData_ByPhone_ReturnsCustomer()
        {
            var phone = "+79161234567";
            var expected = new Customer { Id = Guid.NewGuid().ToString(), Name = "Тест", Phone = phone, Email = "test@mail.ru" };
            _customerStorageMock.Setup(x => x.GetElementByPhone(phone)).Returns(expected);

            var result = _customerBusinessLogic.GetCustomerByData(phone);

            Assert.That(result, Is.EqualTo(expected));
            _customerStorageMock.Verify(x => x.GetElementByPhone(phone), Times.Once);
        }

        [Test]
        public void GetCustomerByData_EmptyData_ThrowsArgumentNullException()
        {
            Assert.That(() => _customerBusinessLogic.GetCustomerByData(null), Throws.ArgumentNullException);
            Assert.That(() => _customerBusinessLogic.GetCustomerByData(""), Throws.ArgumentNullException);
        }

        [Test]
        public void AddCustomer_ValidCustomer_CallsStorage()
        {
            var customer = new Customer { Id = Guid.NewGuid().ToString(), Name = "Новый", Phone = "+79161234567", Email = "new@mail.ru" };
            var called = false;
            _customerStorageMock.Setup(x => x.AddElement(It.IsAny<Customer>()))
                .Callback(() => called = true);

            _customerBusinessLogic.AddCustomer(customer);

            Assert.That(called, Is.True);
            _customerStorageMock.Verify(x => x.AddElement(customer), Times.Once);
        }

        [Test]
        public void AddCustomer_NullCustomer_ThrowsArgumentNullException()
        {
            Assert.That(() => _customerBusinessLogic.AddCustomer(null), Throws.ArgumentNullException);
        }

        [Test]
        public void DeleteCustomer_ValidId_CallsStorage()
        {
            var id = Guid.NewGuid().ToString();
            var called = false;
            _customerStorageMock.Setup(x => x.DeleteElement(It.IsAny<string>()))
                .Callback(() => called = true);

            _customerBusinessLogic.DeleteCustomer(id);

            Assert.That(called, Is.True);
            _customerStorageMock.Verify(x => x.DeleteElement(id), Times.Once);
        }

        [Test]
        public void DeleteCustomer_EmptyId_ThrowsArgumentNullException()
        {
            Assert.That(() => _customerBusinessLogic.DeleteCustomer(null), Throws.ArgumentNullException);
            Assert.That(() => _customerBusinessLogic.DeleteCustomer(""), Throws.ArgumentNullException);
        }
    }
}