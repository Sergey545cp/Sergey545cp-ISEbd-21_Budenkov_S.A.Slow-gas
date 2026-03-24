using NUnit.Framework;
using AutoMapper;
using SlowGas.Database.Implementations;
using SlowGas.Database.Models;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using System;

namespace SlowGas.Tests.StoragesContractsTests
{
    [TestFixture]
    public class RequestStorageTests : BaseStorageTest
    {
        private RequestStorage _storage;
        private CustomerStorage _customerStorage;
        private IMapper _mapper;
        private Customer _customer;

        [SetUp]
        public void SetUp()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RequestEntity, Request>();
                cfg.CreateMap<Request, RequestEntity>();
                cfg.CreateMap<CustomerEntity, Customer>();
                cfg.CreateMap<Customer, CustomerEntity>();
            });
            _mapper = config.CreateMapper();

            _storage = new RequestStorage(_dbContext, _mapper);
            _customerStorage = new CustomerStorage(_dbContext, _mapper);

            _customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Тестовый клиент",
                Phone = "79161234567",
                Email = "test@test.ru"
            };
            _customerStorage.AddElement(_customer);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [Test]
        public void Add_ValidRequest_ShouldSucceed()
        {
            var request = new Request
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _customer.Id,
                Date = DateTime.UtcNow,
                Status = OrderStatus.New
            };
            _storage.AddElement(request);
            var result = _storage.GetElementById(request.Id);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Cancel_ShouldUpdateStatus()
        {
            var request = new Request
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _customer.Id,
                Date = DateTime.UtcNow,
                Status = OrderStatus.New
            };
            _storage.AddElement(request);
            _storage.CancelElement(request.Id);
            var result = _storage.GetElementById(request.Id);
            Assert.That(result.Status, Is.EqualTo(OrderStatus.Cancelled));
        }
    }
}