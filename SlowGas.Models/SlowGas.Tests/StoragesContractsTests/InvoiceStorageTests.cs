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
    public class InvoiceStorageTests : BaseStorageTest
    {
        private InvoiceStorage _storage;
        private ShipmentStorage _shipmentStorage;
        private RequestStorage _requestStorage;
        private CustomerStorage _customerStorage;
        private IMapper _mapper;
        private Customer _customer;
        private Request _request;
        private Shipment _shipment;

        [SetUp]
        public void SetUp()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<InvoiceEntity, Invoice>();
                cfg.CreateMap<Invoice, InvoiceEntity>();
                cfg.CreateMap<ShipmentEntity, Shipment>();
                cfg.CreateMap<Shipment, ShipmentEntity>();
                cfg.CreateMap<RequestEntity, Request>();
                cfg.CreateMap<Request, RequestEntity>();
                cfg.CreateMap<CustomerEntity, Customer>();
                cfg.CreateMap<Customer, CustomerEntity>();
            });
            _mapper = config.CreateMapper();

            _storage = new InvoiceStorage(_dbContext, _mapper);
            _shipmentStorage = new ShipmentStorage(_dbContext, _mapper);
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

            _shipment = new Shipment
            {
                Id = Guid.NewGuid().ToString(),
                RequestId = _request.Id,
                Date = DateTime.UtcNow,
                Status = OrderStatus.Shipped,
                ShippingCost = 5000
            };
            _shipmentStorage.AddElement(_shipment);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [Test]
        public void Add_ValidInvoice_ShouldSucceed()
        {
            var invoice = new Invoice
            {
                Id = Guid.NewGuid().ToString(),
                ShipmentId = _shipment.Id,
                Amount = 300000,
                Status = PaymentStatus.Pending
            };
            _storage.AddElement(invoice);
            var result = _storage.GetElementById(invoice.Id);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Add_DuplicateId_ShouldThrowElementExistsException()
        {
            var id = Guid.NewGuid().ToString();
            var invoice1 = new Invoice
            {
                Id = id,
                ShipmentId = _shipment.Id,
                Amount = 300000,
                Status = PaymentStatus.Pending
            };
            var invoice2 = new Invoice
            {
                Id = id,
                ShipmentId = _shipment.Id,
                Amount = 400000,
                Status = PaymentStatus.Pending
            };
            _storage.AddElement(invoice1);
            Assert.That(() => _storage.AddElement(invoice2), Throws.TypeOf<ElementExistsException>());
        }

        [Test]
        public void GetById_WhenHaveRecord_ShouldReturnCorrectInvoice()
        {
            var invoice = new Invoice
            {
                Id = Guid.NewGuid().ToString(),
                ShipmentId = _shipment.Id,
                Amount = 300000,
                Status = PaymentStatus.Pending
            };
            _storage.AddElement(invoice);
            var result = _storage.GetElementById(invoice.Id);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GetById_WhenNoRecord_ShouldReturnNull()
        {
            var result = _storage.GetElementById(Guid.NewGuid().ToString());
            Assert.That(result, Is.Null);
        }
    }
}