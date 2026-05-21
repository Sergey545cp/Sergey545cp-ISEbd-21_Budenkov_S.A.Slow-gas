using SlowGas.Models.Enums;

namespace SlowGas.Models.BindingModels
{
    public class InvoiceBindingModel
    {
        public string? Id { get; set; }
        public string? ShipmentId { get; set; }
        public decimal? Amount { get; set; }
        public PaymentStatus? Status { get; set; }
    }
}