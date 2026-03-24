using NUnit.Framework;
using AutoMapper;
using SlowGas.Database.Implementations;
using SlowGas.Database.Models;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using System;
using System.Linq;

namespace SlowGas.Tests.StoragesContractsTests
{
    [TestFixture]
    public class MotorStorageTests : BaseStorageTest
    {
        private MotorStorage _storage;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<MotorEntity, Motor>();
                cfg.CreateMap<Motor, MotorEntity>();
            });
            _mapper = config.CreateMapper();
            _storage = new MotorStorage(_dbContext, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [Test]
        public void Add_ValidMotor_ShouldSucceed()
        {
            var motor = new Motor
            {
                Id = Guid.NewGuid().ToString(),
                ModelCode = "V8-001",
                Name = "Мотор V8",
                Type = MotorType.Gasoline,
                Price = 250000
            };
            _storage.AddElement(motor);
            var result = _storage.GetElementById(motor.Id);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Add_DuplicateModelCode_ShouldThrowElementExistsException()
        {
            var motor1 = new Motor
            {
                Id = Guid.NewGuid().ToString(),
                ModelCode = "V8-001",
                Name = "Мотор V8",
                Type = MotorType.Gasoline,
                Price = 250000
            };
            var motor2 = new Motor
            {
                Id = Guid.NewGuid().ToString(),
                ModelCode = "V8-001",
                Name = "Мотор V8 клон",
                Type = MotorType.Gasoline,
                Price = 250000
            };
            _storage.AddElement(motor1);
            Assert.That(() => _storage.AddElement(motor2), Throws.TypeOf<ElementExistsException>());
        }

        [Test]
        public void Delete_SoftDelete_ShouldSetIsActiveFalse()
        {
            var motor = new Motor
            {
                Id = Guid.NewGuid().ToString(),
                ModelCode = "V8-001",
                Name = "Мотор V8",
                Type = MotorType.Gasoline,
                Price = 250000
            };
            _storage.AddElement(motor);

            _storage.DeleteElement(motor.Id);

            // Должен вернуть null т.к. GetElementById теперь фильтрует по IsActive
            var result = _storage.GetElementById(motor.Id);
            Assert.That(result, Is.Null);

            // Проверяем что в БД IsActive = false
            var entity = _dbContext.Motors.Find(motor.Id);
            Assert.That(entity.IsActive, Is.False);
        }

        
    }
}