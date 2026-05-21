using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using SlowGas.Models.BindingModels;
using SlowGas.Models.Enums;
using SlowGas.Models.ViewModels;

namespace SlowGas.Tests.WebApiControllersTests
{
    [TestFixture]
    public class ShipmentsControllerTests : BaseWebApiTest
    {
        private string _requestId;

        [SetUp]
        public async Task Setup()
        {
            _requestId = await CreateTestRequest();
        }

        [Test]
        public async Task GetAll_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/shipments");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Create_ShouldReturnOk()
        {
            if (string.IsNullOrEmpty(_requestId))
            {
                Assert.Inconclusive("Could not create test request");
                return;
            }

            var shipment = new ShipmentBindingModel
            {
                RequestId = _requestId,
                Status = OrderStatus.Shipped,
                ShippingCost = 5000
            };

            var response = await _client.PostAsJsonAsync("/api/shipments", shipment);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK).Or.EqualTo(HttpStatusCode.NoContent),
                $"Expected OK or NoContent, got {response.StatusCode}");
        }

        private async Task<string> CreateTestRequest()
        {
            var uniqueId = Guid.NewGuid().ToString().Substring(0, 8);
            var customer = new CustomerBindingModel
            {
                Name = $"Test Customer {uniqueId}",
                Phone = $"+791612345{uniqueId}",
                Email = $"test_{uniqueId}@test.com"
            };

            await _client.PostAsJsonAsync("/api/customers", customer);

            var customersResponse = await _client.GetAsync("/api/customers");
            var customers = await customersResponse.Content.ReadFromJsonAsync<List<CustomerViewModel>>();
            var customerId = customers?.LastOrDefault()?.Id ?? "";

            if (string.IsNullOrEmpty(customerId))
                return "";

            var request = new RequestBindingModel
            {
                CustomerId = customerId,
                Status = OrderStatus.Approved
            };

            await _client.PostAsJsonAsync("/api/requests", request);

            var requestsResponse = await _client.GetAsync("/api/requests");
            var requests = await requestsResponse.Content.ReadFromJsonAsync<List<RequestViewModel>>();
            return requests?.LastOrDefault()?.Id ?? "";
        }
    }
}