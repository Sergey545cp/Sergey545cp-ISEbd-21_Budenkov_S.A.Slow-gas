using SlowGas.Models.Infrastructure;

namespace SlowGas.WebApi.Infrastructure
{
    public class ConfigurationSalary : IConfigurationSalary
    {
        private readonly IConfiguration _configuration;

        public ConfigurationSalary(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public double ExtraSaleSum => _configuration.GetValue<double>("SalarySettings:ExtraSaleSum");
    }
}