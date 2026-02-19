using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using SlowGas.Models.Infrastructure;

namespace SlowGas.Models.DataModels
{
    public class Motor : IValidation
    {
        public string Id { get; set; }
        public string ModelCode { get; set; }
        public string Name { get; set; }
        public MotorType Type { get; set; }
        public decimal Price { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ValidationException("Id не может быть пустым");

            if (string.IsNullOrWhiteSpace(ModelCode))
                throw new ValidationException("Код модели не может быть пустым");

            if (Price <= 0)
                throw new ValidationException("Цена должна быть больше 0");
        }
    }
}