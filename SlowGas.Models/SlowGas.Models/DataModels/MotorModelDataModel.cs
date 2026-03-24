using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using SlowGas.Models.Infrastructure;

namespace SlowGas.Models.DataModels
{
    public class MotorModelDataModel : IValidation
    {
        public string Id { get; set; } = string.Empty;
        public string ModelCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public MotorType Type { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ValidationException("Id не может быть пустым");

            if (string.IsNullOrWhiteSpace(ModelCode))
                throw new ValidationException("ModelCode не может быть пустым");

            if (string.IsNullOrWhiteSpace(Name))
                throw new ValidationException("Name не может быть пустым");

            if (Price <= 0)
                throw new ValidationException("Price должен быть больше 0");
        }
    }
}