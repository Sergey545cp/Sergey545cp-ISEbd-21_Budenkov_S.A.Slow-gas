using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using SlowGas.Models.BindingModels;
using SlowGas.Models.Enums;

namespace SlowGas.Tests.WebApiControllersTests
{
    [TestFixture]
    public class MotorsControllerTests : BaseWebApiTest
    {
        [Test]
        public async Task GetAll_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/motors");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Create_ShouldReturnOk()
        {
            var uniqueId = Guid.NewGuid().ToString().Substring(0, 8);
            var motor = new MotorBindingModel
            {
                ModelCode = $"TEST-{uniqueId}",
                Name = $"Test Motor {uniqueId}",
                Type = MotorType.Gasoline,
                Price = 100000
            };

            Console.WriteLine($"Creating motor: {System.Text.Json.JsonSerializer.Serialize(motor)}");

            var response = await _client.PostAsJsonAsync("/api/motors", motor);
                
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