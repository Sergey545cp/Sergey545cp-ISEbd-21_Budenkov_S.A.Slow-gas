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
    public class RequestBusinessLogicTests
    {
        private RequestBusinessLogic _requestBusinessLogic;
        private Mock<IRequestStorageContract> _requestStorageMock;
        private Mock<ILogger> _loggerMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _requestStorageMock = new Mock<IRequestStorageContract>();
            _loggerMock = new Mock<ILogger>();
            _requestBusinessLogic = new RequestBusinessLogic(_requestStorageMock.Object, _loggerMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _requestStorageMock.Reset();
        }

        [Test]
        public void GetAllRequests_ReturnsList_WhenStorageHasData()
        {
            var fromDate = new DateTime(2024, 1, 1);
            var toDate = new DateTime(2024, 1, 31);
            var expected = new List<Request>
            {
                new Request { Id = Guid.NewGuid().ToString(), CustomerId = Guid.NewGuid().ToString(), Date = new DateTime(2024, 1, 15), Status = OrderStatus.New },
                new Request { Id = Guid.NewGuid().ToString(), CustomerId = Guid.NewGuid().ToString(), Date = new DateTime(2024, 1, 20), Status = OrderStatus.Approved }
            };
            _requestStorageMock.Setup(x => x.GetList(fromDate, toDate, null)).Returns(expected);

            var result = _requestBusinessLogic.GetAllRequests(fromDate, toDate, null);

            Assert.That(result, Is.EqualTo(expected));
            _requestStorageMock.Verify(x => x.GetList(fromDate, toDate, null), Times.Once);
        }

        [Test]
        public void GetAllRequests_ThrowsIncorrectDatesException_WhenFromDateGreaterThanToDate()
        {
            var fromDate = new DateTime(2024, 2, 1);
            var toDate = new DateTime(2024, 1, 1);
            Assert.That(() => _requestBusinessLogic.GetAllRequests(fromDate, toDate, null), Throws.TypeOf<IncorrectDatesException>());
        }

        [Test]
        public void GetRequestById_ValidId_ReturnsRequest()
        {
            var id = Guid.NewGuid().ToString();
            var expected = new Request { Id = id, CustomerId = Guid.NewGuid().ToString(), Date = DateTime.Now, Status = OrderStatus.New };
            _requestStorageMock.Setup(x => x.GetElementById(id)).Returns(expected);

            var result = _requestBusinessLogic.GetRequestById(id);

            Assert.That(result, Is.EqualTo(expected));
            _requestStorageMock.Verify(x => x.GetElementById(id), Times.Once);
        }

        [Test]
        public void GetRequestById_EmptyId_ThrowsArgumentNullException()
        {
            Assert.That(() => _requestBusinessLogic.GetRequestById(null), Throws.ArgumentNullException);
            Assert.That(() => _requestBusinessLogic.GetRequestById(""), Throws.ArgumentNullException);
        }

        [Test]
        public void CreateRequest_ValidRequest_CallsStorage()
        {
            var request = new Request { Id = Guid.NewGuid().ToString(), CustomerId = Guid.NewGuid().ToString(), Date = DateTime.Now, Status = OrderStatus.New };
            var called = false;
            _requestStorageMock.Setup(x => x.AddElement(It.IsAny<Request>()))
                .Callback(() => called = true);

            _requestBusinessLogic.CreateRequest(request);

            Assert.That(called, Is.True);
            _requestStorageMock.Verify(x => x.AddElement(request), Times.Once);
        }

        [Test]
        public void CreateRequest_NullRequest_ThrowsArgumentNullException()
        {
            Assert.That(() => _requestBusinessLogic.CreateRequest(null), Throws.ArgumentNullException);
        }

        [Test]
        public void CancelRequest_ValidId_CallsStorage()
        {
            var id = Guid.NewGuid().ToString();
            var called = false;
            _requestStorageMock.Setup(x => x.CancelElement(It.IsAny<string>()))
                .Callback(() => called = true);

            _requestBusinessLogic.CancelRequest(id);

            Assert.That(called, Is.True);
            _requestStorageMock.Verify(x => x.CancelElement(id), Times.Once);
        }

        [Test]
        public void CancelRequest_EmptyId_ThrowsArgumentNullException()
        {
            Assert.That(() => _requestBusinessLogic.CancelRequest(null), Throws.ArgumentNullException);
            Assert.That(() => _requestBusinessLogic.CancelRequest(""), Throws.ArgumentNullException);
        }
    }
}