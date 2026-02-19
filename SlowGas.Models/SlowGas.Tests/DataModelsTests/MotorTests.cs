using NUnit.Framework;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;

namespace SlowGas.Tests.DataModelsTests
{
    [TestFixture]
    public class MotorTests
    {
        [Test]
        public void Motor_ValidData_ShouldPass()
        {
            var motor = new Motor
            {
                Id = Guid.NewGuid().ToString(),
                ModelCode = "V8-001",
                Name = "Мотор V8",
                Type = MotorType.Gasoline,
                Price = 250000m
            };

            Assert.That(() => motor.Validate(), Throws.Nothing);
        }

        [Test]
        public void Motor_EmptyId_ShouldThrowException()
        {
            var motor = new Motor
            {
                Id = "",
                ModelCode = "V8-001",
                Name = "Мотор V8",
                Type = MotorType.Gasoline,
                Price = 250000m
            };

            Assert.That(() => motor.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Motor_EmptyModelCode_ShouldThrowException()
        {
            var motor = new Motor
            {
                Id = Guid.NewGuid().ToString(),
                ModelCode = "",
                Name = "Мотор V8",
                Type = MotorType.Gasoline,
                Price = 250000m
            };

            Assert.That(() => motor.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Motor_EmptyName_ShouldThrowException()
        {
            var motor = new Motor
            {
                Id = Guid.NewGuid().ToString(),
                ModelCode = "V8-001",
                Name = "",
                Type = MotorType.Gasoline,
                Price = 250000m
            };

            Assert.That(() => motor.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Motor_ZeroPrice_ShouldThrowException()
        {
            var motor = new Motor
            {
                Id = Guid.NewGuid().ToString(),
                ModelCode = "V8-001",
                Name = "Мотор V8",
                Type = MotorType.Gasoline,
                Price = 0
            };

            Assert.That(() => motor.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Motor_NegativePrice_ShouldThrowException()
        {
            var motor = new Motor
            {
                Id = Guid.NewGuid().ToString(),
                ModelCode = "V8-001",
                Name = "Мотор V8",
                Type = MotorType.Gasoline,
                Price = -1000m
            };

            Assert.That(() => motor.Validate(), Throws.TypeOf<ValidationException>());
        }
    }
}