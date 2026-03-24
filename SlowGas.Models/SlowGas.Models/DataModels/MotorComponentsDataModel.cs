using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using SlowGas.Models.Infrastructure;

namespace SlowGas.Models.DataModels
{
    public class MotorComponentsDataModel : IValidation
    {
        public string MotorModelId { get; set; } = string.Empty;
        public string ComponentId { get; set; } = string.Empty;
        public int QuantityRequired { get; set; }
        public bool IsRequired { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(MotorModelId))
                throw new ValidationException("MotorModelId не может быть пустым");

            if (string.IsNullOrWhiteSpace(ComponentId))
                throw new ValidationException("ComponentId не может быть пустым");

            if (QuantityRequired <= 0)
                throw new ValidationException("QuantityRequired должен быть больше 0");
        }
    }
}