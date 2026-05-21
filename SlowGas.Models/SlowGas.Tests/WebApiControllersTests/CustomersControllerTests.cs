using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using SlowGas.Models.BindingModels;

namespace SlowGas.Tests.WebApiControllersTests
{
    [TestFixture]
    public class CustomersControllerTests : BaseWebApiTest
    {
        [Test]
        public async Task GetAll_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/customers");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Create_ShouldReturnOk()
        {
            var uniqueId = Guid.NewGuid().ToString().Substring(0, 8);
            var customer = new CustomerBindingModel
            {
                Name = $"Test User {uniqueId}",
                Phone = $"+791612345{uniqueId}",
                Email = $"test_{uniqueId}@test.com"
            };

            Console.WriteLine($"Creating customer: {System.Text.Json.JsonSerializer.Serialize(customer)}");

            var response = await _client.PostAsJsonAsync("/api/customers", customer);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"ERROR: Status={response.StatusCode}, Response={error}");
            }

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK).Or.EqualTo(HttpStatusCode.NoContent),
                $"Response: {await response.Content.ReadAsStringAsync()}");
        }
    }
}