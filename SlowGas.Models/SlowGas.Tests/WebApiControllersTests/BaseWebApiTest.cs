using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using NUnit.Framework;
using SlowGas.WebApi;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace SlowGas.Tests.WebApiControllersTests
{
    public class BaseWebApiTest
    {
        protected HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Отключаем реальную аутентификацию для тестов
                        services.AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = "TestScheme";
                            options.DefaultChallengeScheme = "TestScheme";
                        })
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
                    });
                });

            _client = factory.CreateClient();
        }
    }

    // Тестовый обработчик аутентификации
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            System.Text.Encodings.Web.UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
                new Claim(ClaimTypes.Name, "test-user")
            };
            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}