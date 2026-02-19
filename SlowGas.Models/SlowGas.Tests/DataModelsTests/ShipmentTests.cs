using NUnit.Framework;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;

namespace SlowGas.Tests.DataModelsTests
{
    [TestFixture]
    public class ShipmentTests
    {
        [Test]
        public void Shipment_ValidData_ShouldPass()
        {
            var shipment = new Shipment
            {
                Id = Guid.NewGuid().ToString(),
                RequestId = Guid.NewGuid().ToString(),
                Date = DateTime.Now.AddDays(-1),
                Status = OrderStatus.Shipped
            };

            Assert.That(() => shipment.Validate(), Throws.Nothing);
        }

        [Test]
        public void Shipment_EmptyId_ShouldThrowException()
        {
            var shipment = new Shipment
            {
                Id = "",
                RequestId = Guid.NewGuid().ToString(),
                Date = DateTime.Now.AddDays(-1),
                Status = OrderStatus.Shipped
            };

            Assert.That(() => shipment.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Shipment_EmptyRequestId_ShouldThrowException()
        {
            var shipment = new Shipment
            {
                Id = Guid.NewGuid().ToString(),
                RequestId = "",
                Date = DateTime.Now.AddDays(-1),
                Status = OrderStatus.Shipped
            };

            Assert.That(() => shipment.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Shipment_FutureDate_ShouldThrowException()
        {
            var shipment = new Shipment
            {
                Id = Guid.NewGuid().ToString(),
                RequestId = Guid.NewGuid().ToString(),
                Date = DateTime.Now.AddDays(1),
                Status = OrderStatus.Shipped
            };

            Assert.That(() => shipment.Validate(), Throws.TypeOf<ValidationException>());
        }
    }
}