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
    public class InvoiceBusinessLogicTests
    {
        private InvoiceBusinessLogic _invoiceBusinessLogic;
        private Mock<IInvoiceStorageContract> _invoiceStorageMock;
        private Mock<IShipmentStorageContract> _shipmentStorageMock;
        private Mock<ILogger> _loggerMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _invoiceStorageMock = new Mock<IInvoiceStorageContract>();
            _shipmentStorageMock = new Mock<IShipmentStorageContract>();
            _loggerMock = new Mock<ILogger>();
            _invoiceBusinessLogic = new InvoiceBusinessLogic(
                _invoiceStorageMock.Object, _shipmentStorageMock.Object, _loggerMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _invoiceStorageMock.Reset();
            _shipmentStorageMock.Reset();
        }

        [Test]
        public void GetAllInvoices_ReturnsList_WhenStorageHasData()
        {
            var fromDate = new DateTime(2024, 1, 1);
            var toDate = new DateTime(2024, 1, 31);
            var expected = new List<Invoice>
            {
                new Invoice { Id = Guid.NewGuid().ToString(), ShipmentId = Guid.NewGuid().ToString(), Amount = 500000, Status = PaymentStatus.Paid },
                new Invoice { Id = Guid.NewGuid().ToString(), ShipmentId = Guid.NewGuid().ToString(), Amount = 750000, Status = PaymentStatus.Pending }
            };
            _invoiceStorageMock.Setup(x => x.GetList(fromDate, toDate, null)).Returns(expected);

            var result = _invoiceBusinessLogic.GetAllInvoices(fromDate, toDate, null);

            Assert.That(result, Is.EqualTo(expected));
            _invoiceStorageMock.Verify(x => x.GetList(fromDate, toDate, null), Times.Once);
        }

        [Test]
        public void GetAllInvoices_ThrowsIncorrectDatesException_WhenFromDateGreaterThanToDate()
        {
            var fromDate = new DateTime(2024, 2, 1);
            var toDate = new DateTime(2024, 1, 1);
            Assert.That(() => _invoiceBusinessLogic.GetAllInvoices(fromDate, toDate, null), Throws.TypeOf<IncorrectDatesException>());
        }

        [Test]
        public void GetInvoiceById_ValidId_ReturnsInvoice()
        {
            var id = Guid.NewGuid().ToString();
            var expected = new Invoice { Id = id, ShipmentId = Guid.NewGuid().ToString(), Amount = 500000, Status = PaymentStatus.Pending };
            _invoiceStorageMock.Setup(x => x.GetElementById(id)).Returns(expected);

            var result = _invoiceBusinessLogic.GetInvoiceById(id);

            Assert.That(result, Is.EqualTo(expected));
            _invoiceStorageMock.Verify(x => x.GetElementById(id), Times.Once);
        }

        [Test]
        public void GetInvoiceById_EmptyId_ThrowsArgumentNullException()
        {
            Assert.That(() => _invoiceBusinessLogic.GetInvoiceById(null), Throws.ArgumentNullException);
            Assert.That(() => _invoiceBusinessLogic.GetInvoiceById(""), Throws.ArgumentNullException);
        }

        [Test]
        public void GetInvoiceByShipmentId_ValidShipmentId_ReturnsInvoice()
        {
            var shipmentId = Guid.NewGuid().ToString();
            var expected = new Invoice { Id = Guid.NewGuid().ToString(), ShipmentId = shipmentId, Amount = 500000, Status = PaymentStatus.Pending };
            _invoiceStorageMock.Setup(x => x.GetElementByShipmentId(shipmentId)).Returns(expected);

            var result = _invoiceBusinessLogic.GetInvoiceByShipmentId(shipmentId);

            Assert.That(result, Is.EqualTo(expected));
            _invoiceStorageMock.Verify(x => x.GetElementByShipmentId(shipmentId), Times.Once);
        }

        [Test]
        public void CreateInvoice_ValidInvoice_CallsStorage()
        {
            var shipmentId = Guid.NewGuid().ToString();
            var invoice = new Invoice { Id = Guid.NewGuid().ToString(), ShipmentId = shipmentId, Amount = 500000, Status = PaymentStatus.Pending };
            _shipmentStorageMock.Setup(x => x.GetElementById(shipmentId)).Returns(new Shipment { Id = shipmentId });
            var called = false;
            _invoiceStorageMock.Setup(x => x.AddElement(It.IsAny<Invoice>()))
                .Callback(() => called = true);

            _invoiceBusinessLogic.CreateInvoice(invoice);

            Assert.That(called, Is.True);
            _invoiceStorageMock.Verify(x => x.AddElement(invoice), Times.Once);
        }

        [Test]
        public void CreateInvoice_ShipmentNotFound_ThrowsElementNotFoundException()
        {
            var invoice = new Invoice { Id = Guid.NewGuid().ToString(), ShipmentId = Guid.NewGuid().ToString(), Amount = 500000, Status = PaymentStatus.Pending };
            _shipmentStorageMock.Setup(x => x.GetElementById(invoice.ShipmentId)).Returns((Shipment)null);
            Assert.That(() => _invoiceBusinessLogic.CreateInvoice(invoice), Throws.TypeOf<ElementNotFoundException>());
        }

        [Test]
        public void PayInvoice_ValidId_CallsStorage()
        {
            var id = Guid.NewGuid().ToString();
            var called = false;
            _invoiceStorageMock.Setup(x => x.PayInvoice(It.IsAny<string>()))
                .Callback(() => called = true);

            _invoiceBusinessLogic.PayInvoice(id);

            Assert.That(called, Is.True);
            _invoiceStorageMock.Verify(x => x.PayInvoice(id), Times.Once);
        }

        [Test]
        public void PayInvoice_EmptyId_ThrowsArgumentNullException()
        {
            Assert.That(() => _invoiceBusinessLogic.PayInvoice(null), Throws.ArgumentNullException);
            Assert.That(() => _invoiceBusinessLogic.PayInvoice(""), Throws.ArgumentNullException);
        }
    }
}