namespace SlowGas.Models.Infrastructure.PostConfigurations
{
    public class SupervisorPostConfiguration : PostConfiguration
    {
        public override string Type => nameof(SupervisorPostConfiguration);
        public double PersonalCountTrendPremium { get; set; }
    }
}