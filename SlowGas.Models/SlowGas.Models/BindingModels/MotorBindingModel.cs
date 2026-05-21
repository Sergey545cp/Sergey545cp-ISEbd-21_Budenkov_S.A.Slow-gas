using SlowGas.Models.Enums;

namespace SlowGas.Models.BindingModels
{
    public class MotorBindingModel
    {
        public string? Id { get; set; }
        public string? ModelCode { get; set; }
        public string? Name { get; set; }
        public MotorType? Type { get; set; }
        public decimal? Price { get; set; }
    }
}