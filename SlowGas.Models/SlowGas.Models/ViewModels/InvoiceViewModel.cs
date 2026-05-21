using SlowGas.Models.Enums;

namespace SlowGas.Models.ViewModels
{
    public class InvoiceViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string ShipmentId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
    }
}