using SlowGas.Models.Enums;

namespace SlowGas.Models.BindingModels
{
    public class RequestBindingModel
    {
        public string? Id { get; set; }
        public string? CustomerId { get; set; }
        public DateTime? Date { get; set; }
        public OrderStatus? Status { get; set; }
    }
}   