using Moq;
using SlowGas.BusinessLogic.Implementations;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using SlowGas.Contracts.StoragesContracts;

namespace SlowGas.Tests.BusinessLogicsContractsTests
{
    [TestFixture]
    public class ShipmentBusinessLogicTests
    {
        private ShipmentBusinessLogic _shipmentBusinessLogic;
        private Mock<IShipmentStorageContract> _shipmentStorageMock;
        private Mock<IRequestStorageContract> _requestStorageMock;
        private Mock<ILogger> _loggerMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _shipmentStorageMock = new Mock<IShipmentStorageContract>();
            _requestStorageMock = new Mock<IRequestStorageContract>();
            _loggerMock = new Mock<ILogger>();
            _shipmentBusinessLogic = new ShipmentBusinessLogic(
                _shipmentStorageMock.Object, _requestStorageMock.Object, _loggerMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _shipmentStorageMock.Reset();
            _requestStorageMock.Reset();
        }

        [Test]
        public void GetAllShipments_ReturnsList_WhenStorageHasData()
        {
            var fromDate = new DateTime(2024, 1, 1);
            var toDate = new DateTime(2024, 1, 31);
            var expected = new List<Shipment>
            {
                new Shipment { Id = Guid.NewGuid().ToString(), RequestId = Guid.NewGuid().ToString(), Date = new DateTime(2024, 1, 15), Status = OrderStatus.Shipped },
                new Shipment { Id = Guid.NewGuid().ToString(), RequestId = Guid.NewGuid().ToString(), Date = new DateTime(2024, 1, 20), Status = OrderStatus.Shipped }
            };
            _shipmentStorageMock.Setup(x => x.GetList(fromDate, toDate, null)).Returns(expected);

            var result = _shipmentBusinessLogic.GetAllShipments(fromDate, toDate, null);

            Assert.That(result, Is.EqualTo(expected));
            _shipmentStorageMock.Verify(x => x.GetList(fromDate, toDate, null), Times.Once);
        }

        [Test]
        public void GetAllShipments_ThrowsIncorrectDatesException_WhenFromDateGreaterThanToDate()
        {
            var fromDate = new DateTime(2024, 2, 1);
            var toDate = new DateTime(2024, 1, 1);
            Assert.That(() => _shipmentBusinessLogic.GetAllShipments(fromDate, toDate, null), Throws.TypeOf<IncorrectDatesException>());
        }

        [Test]
        public void GetShipmentById_ValidId_ReturnsShipment()
        {
            var id = Guid.NewGuid().ToString();
            var expected = new Shipment { Id = id, RequestId = Guid.NewGuid().ToString(), Date = DateTime.Now, Status = OrderStatus.Shipped };
            _shipmentStorageMock.Setup(x => x.GetElementById(id)).Returns(expected);

            var result = _shipmentBusinessLogic.GetShipmentById(id);

            Assert.That(result, Is.EqualTo(expected));
            _shipmentStorageMock.Verify(x => x.GetElementById(id), Times.Once);
        }

        [Test]
        public void GetShipmentById_EmptyId_ThrowsArgumentNullException()
        {
            Assert.That(() => _shipmentBusinessLogic.GetShipmentById(null), Throws.ArgumentNullException);
            Assert.That(() => _shipmentBusinessLogic.GetShipmentById(""), Throws.ArgumentNullException);
        }

        [Test]
        public void CreateShipment_ValidShipment_CallsStorage()
        {
            var requestId = Guid.NewGuid().ToString();
            var shipment = new Shipment { Id = Guid.NewGuid().ToString(), RequestId = requestId, Date = DateTime.Now, Status = OrderStatus.Shipped };
            _requestStorageMock.Setup(x => x.GetElementById(requestId)).Returns(new Request { Id = requestId });
            var called = false;
            _shipmentStorageMock.Setup(x => x.AddElement(It.IsAny<Shipment>()))
                .Callback(() => called = true);

            _shipmentBusinessLogic.CreateShipment(shipment);

            Assert.That(called, Is.True);
            _shipmentStorageMock.Verify(x => x.AddElement(shipment), Times.Once);
        }

        [Test]
        public void CreateShipment_RequestNotFound_ThrowsElementNotFoundException()
        {
            var shipment = new Shipment { Id = Guid.NewGuid().ToString(), RequestId = Guid.NewGuid().ToString(), Date = DateTime.Now, Status = OrderStatus.Shipped };
            _requestStorageMock.Setup(x => x.GetElementById(shipment.RequestId)).Returns((Request)null);
            Assert.That(() => _shipmentBusinessLogic.CreateShipment(shipment), Throws.TypeOf<ElementNotFoundException>());
        }

        [Test]
        public void CalculateMonthlyShipments_ReturnsShipmentsForCustomer()
        {
            var customerId = Guid.NewGuid().ToString();
            var month = new DateTime(2024, 5, 1);
            var requests = new List<Request>
            {
                new Request { Id = Guid.NewGuid().ToString(), CustomerId = customerId, Date = new DateTime(2024, 5, 10), Status = OrderStatus.Approved }
            };
            var shipments = new List<Shipment>
            {
                new Shipment { Id = Guid.NewGuid().ToString(), RequestId = requests[0].Id, Date = new DateTime(2024, 5, 15), Status = OrderStatus.Shipped }
            };

            _requestStorageMock.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), customerId)).Returns(requests);
            _shipmentStorageMock.Setup(x => x.GetList(null, null, requests[0].Id)).Returns(shipments);

            var result = _shipmentBusinessLogic.CalculateMonthlyShipments(customerId, month);

            Assert.That(result, Has.Count.EqualTo(1));
        }
    }
}