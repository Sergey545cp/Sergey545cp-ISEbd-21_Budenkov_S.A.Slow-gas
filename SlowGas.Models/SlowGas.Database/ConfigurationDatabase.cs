using SlowGas.Models.Infrastructure;

namespace SlowGas.Database
{
    public class ConfigurationDatabase : IConfigurationDatabase
    {
        public string ConnectionString { get; }

        public ConfigurationDatabase(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }

    public class TestConfigurationDatabase : IConfigurationDatabase
    {
        public string ConnectionString => "Host=localhost;Database=SlowGasTest;Username=postgres;Password=postgres";
    }
}