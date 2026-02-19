using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using SlowGas.Models.Infrastructure;

namespace SlowGas.Models.DataModels
{
    public class Shipment : IValidation
    {
        public string Id { get; set; }
        public string RequestId { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus Status { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ValidationException("Id не может быть пустым");

            if (string.IsNullOrWhiteSpace(RequestId))
                throw new ValidationException("RequestId не может быть пустым");

            if (Date > DateTime.Now)
                throw new ValidationException("Дата не может быть в будущем");
        }
    }
}