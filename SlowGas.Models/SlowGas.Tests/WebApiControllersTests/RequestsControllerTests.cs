using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using SlowGas.Models.BindingModels;
using SlowGas.Models.Enums;
using SlowGas.Models.ViewModels;

namespace SlowGas.Tests.WebApiControllersTests
{
    [TestFixture]
    public class RequestsControllerTests : BaseWebApiTest
    {
        private string _customerId;

        [SetUp]
        public async Task Setup()
        {
            _customerId = await CreateTestCustomer();
        }

        [Test]
        public async Task GetAll_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/requests");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Create_ShouldReturnOk()
        {
            if (string.IsNullOrEmpty(_customerId))
            {
                Assert.Inconclusive("Could not create test customer");
                return;
            }

            var request = new RequestBindingModel
            {
                CustomerId = _customerId,
                Status = OrderStatus.New
            };

            var response = await _client.PostAsJsonAsync("/api/requests", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK).Or.EqualTo(HttpStatusCode.NoContent),
                $"Expected OK or NoContent, got {response.StatusCode}");
        }

        private async Task<string> CreateTestCustomer()
        {
            var uniqueId = Guid.NewGuid().ToString().Substring(0, 8);
            var customer = new CustomerBindingModel
            {
                Name = $"Test Customer {uniqueId}",
                Phone = $"+791612345{uniqueId}",
                Email = $"test_{uniqueId}@test.com"
            };

            await _client.PostAsJsonAsync("/api/customers", customer);

            var response = await _client.GetAsync("/api/customers");
            var customers = await response.Content.ReadFromJsonAsync<List<CustomerViewModel>>();
            return customers?.LastOrDefault()?.Id ?? "";
        }
    }
}