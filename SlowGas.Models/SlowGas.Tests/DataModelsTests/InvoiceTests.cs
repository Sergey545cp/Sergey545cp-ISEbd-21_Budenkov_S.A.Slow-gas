using NUnit.Framework;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;

namespace SlowGas.Tests.DataModelsTests
{
    [TestFixture]
    public class InvoiceTests
    {
        [Test]
        public void Invoice_ValidData_ShouldPass()
        {
            var invoice = new Invoice
            {
                Id = Guid.NewGuid().ToString(),
                ShipmentId = Guid.NewGuid().ToString(),
                Amount = 50000m,
                Status = PaymentStatus.Pending
            };

            Assert.That(() => invoice.Validate(), Throws.Nothing);
        }

        [Test]
        public void Invoice_EmptyId_ShouldThrowException()
        {
            var invoice = new Invoice
            {
                Id = "",
                ShipmentId = Guid.NewGuid().ToString(),
                Amount = 50000m,
                Status = PaymentStatus.Pending
            };

            Assert.That(() => invoice.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Invoice_EmptyShipmentId_ShouldThrowException()
        {
            var invoice = new Invoice
            {
                Id = Guid.NewGuid().ToString(),
                ShipmentId = "",
                Amount = 50000m,
                Status = PaymentStatus.Pending
            };

            Assert.That(() => invoice.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Invoice_ZeroAmount_ShouldThrowException()
        {
            var invoice = new Invoice
            {
                Id = Guid.NewGuid().ToString(),
                ShipmentId = Guid.NewGuid().ToString(),
                Amount = 0,
                Status = PaymentStatus.Pending
            };

            Assert.That(() => invoice.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Invoice_NegativeAmount_ShouldThrowException()
        {
            var invoice = new Invoice
            {
                Id = Guid.NewGuid().ToString(),
                ShipmentId = Guid.NewGuid().ToString(),
                Amount = -1000m,
                Status = PaymentStatus.Pending
            };

            Assert.That(() => invoice.Validate(), Throws.TypeOf<ValidationException>());
        }
    }
}