using SlowGas.Models.Infrastructure;

namespace SlowGas.WebApi.Infrastructure
{
    public class ConfigurationDatabase : IConfigurationDatabase
    {
        private readonly IConfiguration _configuration;

        public ConfigurationDatabase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found");
    }
}