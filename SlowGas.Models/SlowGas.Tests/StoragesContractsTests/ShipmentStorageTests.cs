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
    public class ShipmentStorageTests : BaseStorageTest
    {
        private ShipmentStorage _storage;
        private RequestStorage _requestStorage;
        private CustomerStorage _customerStorage;
        private IMapper _mapper;
        private Customer _customer;
        private Request _request;

        [SetUp]
        public void SetUp()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<ShipmentEntity, Shipment>();
                cfg.CreateMap<Shipment, ShipmentEntity>();
                cfg.CreateMap<RequestEntity, Request>();
                cfg.CreateMap<Request, RequestEntity>();
                cfg.CreateMap<CustomerEntity, Customer>();
                cfg.CreateMap<Customer, CustomerEntity>();
            });
            _mapper = config.CreateMapper();

            _storage = new ShipmentStorage(_dbContext, _mapper);
            _requestStorage = new RequestStorage(_dbContext, _mapper);
            _customerStorage = new CustomerStorage(_dbContext, _mapper);

            _customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Тестовый клиент",
                Phone = "79161234567",
                Email = "test@test.ru"
            };
            _customerStorage.AddElement(_customer);

            _request = new Request
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _customer.Id,
                Date = DateTime.UtcNow,
                Status = OrderStatus.Approved
            };
            _requestStorage.AddElement(_request);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [Test]
        public void Add_ValidShipment_ShouldSucceed()
        {
            var shipment = new Shipment
            {
                Id = Guid.NewGuid().ToString(),
                RequestId = _request.Id,
                Date = DateTime.UtcNow,
                Status = OrderStatus.Shipped,
                ShippingCost = 5000
            };
            _storage.AddElement(shipment);
            var result = _storage.GetElementById(shipment.Id);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Cancel_ShouldUpdateStatus()
        {
            var shipment = new Shipment
            {
                Id = Guid.NewGuid().ToString(),
                RequestId = _request.Id,
                Date = DateTime.UtcNow,
                Status = OrderStatus.Shipped,
                ShippingCost = 5000
            };
            _storage.AddElement(shipment);
            _storage.CancelElement(shipment.Id);
            var result = _storage.GetElementById(shipment.Id);
            Assert.That(result.Status, Is.EqualTo(OrderStatus.Cancelled));
        }
    }
}