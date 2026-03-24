using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using SlowGas.Models.Infrastructure;

namespace SlowGas.Models.DataModels
{
    public class ComponentDataModel : IValidation
    {
        public string Id { get; set; } = string.Empty;
        public string ComponentCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Specification { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public int StockQuantity { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ValidationException("Id не может быть пустым");

            if (string.IsNullOrWhiteSpace(ComponentCode))
                throw new ValidationException("ComponentCode не может быть пустым");

            if (string.IsNullOrWhiteSpace(Name))
                throw new ValidationException("Name не может быть пустым");

            if (Cost < 0)
                throw new ValidationException("Cost не может быть отрицательным");

            if (StockQuantity < 0)
                throw new ValidationException("StockQuantity не может быть отрицательным");
        }
    }
}