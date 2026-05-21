using SlowGas.Models.Enums;

namespace SlowGas.Models.ViewModels
{
    public class RequestViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public OrderStatus Status { get; set; }
    }
}