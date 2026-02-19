using NUnit.Framework;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;

namespace SlowGas.Tests.DataModelsTests
{
    [TestFixture]
    public class RequestTests
    {
        [Test]
        public void Request_ValidData_ShouldPass()
        {
            var request = new Request
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = Guid.NewGuid().ToString(),
                Date = DateTime.Now.AddDays(-1),
                Status = OrderStatus.New
            };

            Assert.That(() => request.Validate(), Throws.Nothing);
        }

        [Test]
        public void Request_EmptyId_ShouldThrowException()
        {
            var request = new Request
            {
                Id = "",
                CustomerId = Guid.NewGuid().ToString(),
                Date = DateTime.Now.AddDays(-1),
                Status = OrderStatus.New
            };

            Assert.That(() => request.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Request_EmptyCustomerId_ShouldThrowException()
        {
            var request = new Request
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = "",
                Date = DateTime.Now.AddDays(-1),
                Status = OrderStatus.New
            };

            Assert.That(() => request.Validate(), Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void Request_FutureDate_ShouldThrowException()
        {
            var request = new Request
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = Guid.NewGuid().ToString(),
                Date = DateTime.Now.AddDays(1),
                Status = OrderStatus.New
            };

            Assert.That(() => request.Validate(), Throws.TypeOf<ValidationException>());
        }
    }
}