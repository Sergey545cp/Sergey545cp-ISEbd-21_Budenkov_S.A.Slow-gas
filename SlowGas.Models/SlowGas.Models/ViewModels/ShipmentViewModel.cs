using SlowGas.Models.Enums;

namespace SlowGas.Models.ViewModels
{
    public class ShipmentViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public OrderStatus Status { get; set; }
        public decimal ShippingCost { get; set; }
    }
}   