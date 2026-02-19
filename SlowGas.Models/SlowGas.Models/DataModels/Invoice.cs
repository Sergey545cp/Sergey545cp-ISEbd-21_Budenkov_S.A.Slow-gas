using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using SlowGas.Models.Infrastructure;

namespace SlowGas.Models.DataModels
{
    public class Invoice : IValidation
    {
        public string Id { get; set; }
        public string ShipmentId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ValidationException("Id не может быть пустым");

            if (string.IsNullOrWhiteSpace(ShipmentId))
                throw new ValidationException("ShipmentId не может быть пустым");

            if (Amount <= 0)
                throw new ValidationException("Сумма должна быть больше 0");
        }
    }
}