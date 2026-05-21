using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using SlowGas.Models.BindingModels;
using SlowGas.Models.Enums;
using SlowGas.Models.ViewModels;

namespace SlowGas.Tests.WebApiControllersTests
{
    [TestFixture]
    public class InvoicesControllerTests : BaseWebApiTest
    {
        [Test]
        public async Task GetAll_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/invoices");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Create_ShouldReturnOk()
        {
            var customerId = await CreateTestCustomer();
            if (string.IsNullOrEmpty(customerId))
            {
                Assert.Inconclusive("Could not create test customer");
                return;
            }

            var requestId = await CreateTestRequest(customerId);
            if (string.IsNullOrEmpty(requestId))
            {
                Assert.Inconclusive("Could not create test request");
                return;
            }

            var shipmentId = await CreateTestShipment(requestId);
            if (string.IsNullOrEmpty(shipmentId))
            {
                Assert.Inconclusive("Could not create test shipment");
                return;
            }

            var invoice = new InvoiceBindingModel
            {
                ShipmentId = shipmentId,
                Amount = 300000,
                Status = PaymentStatus.Pending
            };

            var response = await _client.PostAsJsonAsync("/api/invoices", invoice);

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

        private async Task<string> CreateTestRequest(string customerId)
        {
            var request = new RequestBindingModel
            {
                CustomerId = customerId,
                Status = OrderStatus.Approved
            };

            await _client.PostAsJsonAsync("/api/requests", request);

            var response = await _client.GetAsync("/api/requests");
            var requests = await response.Content.ReadFromJsonAsync<List<RequestViewModel>>();
            return requests?.LastOrDefault()?.Id ?? "";
        }

        private async Task<string> CreateTestShipment(string requestId)
        {
            var shipment = new ShipmentBindingModel
            {
                RequestId = requestId,
                Status = OrderStatus.Shipped,
                ShippingCost = 5000
            };

            await _client.PostAsJsonAsync("/api/shipments", shipment);

            var response = await _client.GetAsync("/api/shipments");
            var shipments = await response.Content.ReadFromJsonAsync<List<ShipmentViewModel>>();
            return shipments?.LastOrDefault()?.Id ?? "";
        }
    }
}