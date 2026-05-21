namespace SlowGas.Models.Infrastructure.PostConfigurations
{
    public class CashierPostConfiguration : PostConfiguration
    {
        public override string Type => nameof(CashierPostConfiguration);
        public double SalePercent { get; set; }
        public double BonusForExtraSales { get; set; }
    }
}