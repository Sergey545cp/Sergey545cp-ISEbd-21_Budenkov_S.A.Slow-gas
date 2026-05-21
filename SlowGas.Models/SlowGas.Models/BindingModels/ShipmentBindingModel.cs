using SlowGas.Models.Enums;

namespace SlowGas.Models.BindingModels
{
    public class ShipmentBindingModel
    {
        public string? Id { get; set; }
        public string? RequestId { get; set; }
        public DateTime? Date { get; set; }
        public OrderStatus? Status { get; set; }
        public decimal? ShippingCost { get; set; }
    }
}