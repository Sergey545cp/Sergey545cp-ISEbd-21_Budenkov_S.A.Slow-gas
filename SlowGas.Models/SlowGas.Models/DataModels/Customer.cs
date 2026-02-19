using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using SlowGas.Models.Infrastructure;

namespace SlowGas.Models.DataModels
{
    public class Customer : IValidation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ValidationException("Id не может быть пустым");

            if (string.IsNullOrWhiteSpace(Name))
                throw new ValidationException("Имя не может быть пустым");

            if (!Phone.IsValidPhone())
                throw new ValidationException("Телефон имеет неверный формат");
        }
    }
}