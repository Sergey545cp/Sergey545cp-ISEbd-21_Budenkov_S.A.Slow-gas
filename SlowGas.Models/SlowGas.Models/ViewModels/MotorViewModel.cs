using SlowGas.Models.Enums;

namespace SlowGas.Models.ViewModels
{
    public class MotorViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string ModelCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public MotorType Type { get; set; }
        public decimal Price { get; set; }
    }
}