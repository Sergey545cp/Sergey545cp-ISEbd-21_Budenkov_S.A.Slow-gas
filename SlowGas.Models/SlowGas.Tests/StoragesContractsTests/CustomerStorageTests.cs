using NUnit.Framework;
using AutoMapper;
using SlowGas.Database.Implementations;
using SlowGas.Database.Models;
using SlowGas.Models.DataModels;
using SlowGas.Models.Exceptions;
using System;
using System.Linq;

namespace SlowGas.Tests.StoragesContractsTests
{
    [TestFixture]
    public class CustomerStorageTests : BaseStorageTest
    {
        private CustomerStorage _storage;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<CustomerEntity, Customer>();
                cfg.CreateMap<Customer, CustomerEntity>();
            });
            _mapper = config.CreateMapper();
            _storage = new CustomerStorage(_dbContext, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [Test]
        public void Add_ValidCustomer_ShouldSucceed()
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "ООО Тест",
                Phone = "+79161234567",  // 12 символов, влезает в 50
                Email = "test@test.ru"
            };
            _storage.AddElement(customer);
            var result = _storage.GetElementById(customer.Id);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Add_DuplicatePhone_ShouldThrowElementExistsException()
        {
            var phone = "+79161234567";
            var customer1 = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "ООО Тест1",
                Phone = phone,
                Email = "test1@test.ru"
            };
            var customer2 = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "ООО Тест2",
                Phone = phone,
                Email = "test2@test.ru"
            };
            _storage.AddElement(customer1);
            Assert.That(() => _storage.AddElement(customer2), Throws.TypeOf<ElementExistsException>());
        }

        [Test]
        public void Add_DuplicateId_ShouldThrowElementExistsException()
        {
            var id = Guid.NewGuid().ToString();
            var customer1 = new Customer
            {
                Id = id,
                Name = "ООО Тест1",
                Phone = "+79161234561",
                Email = "test1@test.ru"
            };
            var customer2 = new Customer
            {
                Id = id,
                Name = "ООО Тест2",
                Phone = "+79161234562",
                Email = "test2@test.ru"
            };
            _storage.AddElement(customer1);
            Assert.That(() => _storage.AddElement(customer2), Throws.TypeOf<ElementExistsException>());
        }

        [Test]
        public void GetList_WhenHaveRecords_ShouldReturnAllCustomers()
        {
            var customer1 = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "ООО Тест1",
                Phone = "+79161234561",
                Email = "test1@test.ru"
            };
            var customer2 = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "ООО Тест2",
                Phone = "+79161234562",
                Email = "test2@test.ru"
            };
            _storage.AddElement(customer1);
            _storage.AddElement(customer2);
            var list = _storage.GetList();
            Assert.That(list, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetList_WhenNoRecords_ShouldReturnEmptyList()
        {
            var list = _storage.GetList();
            Assert.That(list, Is.Empty);
        }

        [Test]
        public void GetById_WhenHaveRecord_ShouldReturnCorrectCustomer()
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Тест",
                Phone = "+79161234567",
                Email = "test@test.ru"
            };
            _storage.AddElement(customer);
            var result = _storage.GetElementById(customer.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(customer.Id));
        }

        [Test]
        public void GetById_WhenNoRecord_ShouldReturnNull()
        {
            var result = _storage.GetElementById(Guid.NewGuid().ToString());
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByName_WhenHaveRecord_ShouldReturnCorrectCustomer()
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Уникальное имя",
                Phone = "+79161234567",
                Email = "test@test.ru"
            };
            _storage.AddElement(customer);
            var result = _storage.GetElementByName("Уникальное имя");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(customer.Id));
        }

        [Test]
        public void GetByPhone_WhenHaveRecord_ShouldReturnCorrectCustomer()
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Тест",
                Phone = "+79161234567",
                Email = "test@test.ru"
            };
            _storage.AddElement(customer);
            var result = _storage.GetElementByPhone("+79161234567");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(customer.Id));
        }

        [Test]
        public void Update_ShouldModifyCustomer()
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Старое имя",
                Phone = "+79161234567",
                Email = "test@test.ru"
            };
            _storage.AddElement(customer);
            customer.Name = "Новое имя";
            _storage.UpdateElement(customer);
            var result = _storage.GetElementById(customer.Id);
            Assert.That(result.Name, Is.EqualTo("Новое имя"));
        }

        [Test]
        public void Update_DuplicatePhone_ShouldThrowElementExistsException()
        {
            var customer1 = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "ООО Тест1",
                Phone = "+79161234561",
                Email = "test1@test.ru"
            };
            var customer2 = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "ООО Тест2",
                Phone = "+79161234562",
                Email = "test2@test.ru"
            };
            _storage.AddElement(customer1);
            _storage.AddElement(customer2);
            customer2.Phone = customer1.Phone;
            Assert.That(() => _storage.UpdateElement(customer2), Throws.TypeOf<ElementExistsException>());
        }

        [Test]
        public void Update_NotFound_ShouldThrowElementNotFoundException()
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Тест",
                Phone = "+79161234567",
                Email = "test@test.ru"
            };
            Assert.That(() => _storage.UpdateElement(customer), Throws.TypeOf<ElementNotFoundException>());
        }

        [Test]
        public void Delete_ShouldRemoveCustomer()
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Тест",
                Phone = "+79161234567",
                Email = "test@test.ru"
            };
            _storage.AddElement(customer);
            _storage.DeleteElement(customer.Id);
            var result = _storage.GetElementById(customer.Id);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Delete_NotFound_ShouldThrowElementNotFoundException()
        {
            Assert.That(() => _storage.DeleteElement(Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
        }
    }
}