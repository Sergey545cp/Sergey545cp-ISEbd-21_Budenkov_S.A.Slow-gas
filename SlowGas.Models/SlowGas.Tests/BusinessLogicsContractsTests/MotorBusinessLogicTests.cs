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
    public class MotorBusinessLogicTests
    {
        private MotorBusinessLogic _motorBusinessLogic;
        private Mock<IMotorStorageContract> _motorStorageMock;
        private Mock<ILogger> _loggerMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _motorStorageMock = new Mock<IMotorStorageContract>();
            _loggerMock = new Mock<ILogger>();
            _motorBusinessLogic = new MotorBusinessLogic(_motorStorageMock.Object, _loggerMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _motorStorageMock.Reset();
        }

        [Test]
        public void GetAllMotors_ReturnsList_WhenStorageHasData()
        {
            var expected = new List<Motor>
            {
                new Motor { Id = Guid.NewGuid().ToString(), Name = "Мотор V8", ModelCode = "V8-001", Type = MotorType.Gasoline, Price = 250000 },
                new Motor { Id = Guid.NewGuid().ToString(), Name = "Мотор D4", ModelCode = "D4-002", Type = MotorType.Diesel, Price = 300000 }
            };
            _motorStorageMock.Setup(x => x.GetList(true)).Returns(expected);

            var result = _motorBusinessLogic.GetAllMotors(true);

            Assert.That(result, Is.EqualTo(expected));
            _motorStorageMock.Verify(x => x.GetList(true), Times.Once);
        }

        [Test]
        public void GetAllMotors_ThrowsNullListException_WhenStorageReturnsNull()
        {
            _motorStorageMock.Setup(x => x.GetList(true)).Returns((List<Motor>)null);
            Assert.That(() => _motorBusinessLogic.GetAllMotors(true), Throws.TypeOf<NullListException>());
        }

        [Test]
        public void GetMotorByData_ById_ReturnsMotor()
        {
            var id = Guid.NewGuid().ToString();
            var expected = new Motor { Id = id, Name = "Мотор V8", ModelCode = "V8-001", Type = MotorType.Gasoline, Price = 250000 };
            _motorStorageMock.Setup(x => x.GetElementById(id)).Returns(expected);

            var result = _motorBusinessLogic.GetMotorByData(id);

            Assert.That(result, Is.EqualTo(expected));
            _motorStorageMock.Verify(x => x.GetElementById(id), Times.Once);
        }

        [Test]
        public void GetMotorByData_ByName_ReturnsMotor()
        {
            var name = "Мотор V8";
            var expected = new Motor { Id = Guid.NewGuid().ToString(), Name = name, ModelCode = "V8-001", Type = MotorType.Gasoline, Price = 250000 };
            _motorStorageMock.Setup(x => x.GetElementByName(name)).Returns(expected);

            var result = _motorBusinessLogic.GetMotorByData(name);

            Assert.That(result, Is.EqualTo(expected));
            _motorStorageMock.Verify(x => x.GetElementByName(name), Times.Once);
        }

        [Test]
        public void GetMotorByData_ByModelCode_ReturnsMotor()
        {
            var modelCode = "V8-001";
            var expected = new Motor { Id = Guid.NewGuid().ToString(), Name = "Мотор V8", ModelCode = modelCode, Type = MotorType.Gasoline, Price = 250000 };
            _motorStorageMock.Setup(x => x.GetElementByModelCode(modelCode)).Returns(expected);

            var result = _motorBusinessLogic.GetMotorByData(modelCode);

            Assert.That(result, Is.EqualTo(expected));
            _motorStorageMock.Verify(x => x.GetElementByModelCode(modelCode), Times.Once);
        }

        [Test]
        public void GetMotorByData_EmptyData_ThrowsArgumentNullException()
        {
            Assert.That(() => _motorBusinessLogic.GetMotorByData(null), Throws.ArgumentNullException);
            Assert.That(() => _motorBusinessLogic.GetMotorByData(""), Throws.ArgumentNullException);
        }

        [Test]
        public void AddMotor_ValidMotor_CallsStorage()
        {
            var motor = new Motor { Id = Guid.NewGuid().ToString(), Name = "Новый мотор", ModelCode = "NEW-001", Type = MotorType.Gasoline, Price = 200000 };
            var called = false;
            _motorStorageMock.Setup(x => x.AddElement(It.IsAny<Motor>()))
                .Callback(() => called = true);

            _motorBusinessLogic.AddMotor(motor);

            Assert.That(called, Is.True);
            _motorStorageMock.Verify(x => x.AddElement(motor), Times.Once);
        }

        [Test]
        public void AddMotor_NullMotor_ThrowsArgumentNullException()
        {
            Assert.That(() => _motorBusinessLogic.AddMotor(null), Throws.ArgumentNullException);
        }

        [Test]
        public void DeleteMotor_ValidId_CallsStorage()
        {
            var id = Guid.NewGuid().ToString();
            var called = false;
            _motorStorageMock.Setup(x => x.DeleteElement(It.IsAny<string>()))
                .Callback(() => called = true);

            _motorBusinessLogic.DeleteMotor(id);

            Assert.That(called, Is.True);
            _motorStorageMock.Verify(x => x.DeleteElement(id), Times.Once);
        }

        [Test]
        public void DeleteMotor_EmptyId_ThrowsArgumentNullException()
        {
            Assert.That(() => _motorBusinessLogic.DeleteMotor(null), Throws.ArgumentNullException);
            Assert.That(() => _motorBusinessLogic.DeleteMotor(""), Throws.ArgumentNullException);
        }
    }
}