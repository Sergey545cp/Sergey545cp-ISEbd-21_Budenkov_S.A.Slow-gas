using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;

namespace SlowGas.Models.DataModels
{
    public class SalaryDataModel
    {
        public string Id { get; private set; }
        public string WorkerId { get; private set; }
        public DateTime Period { get; private set; }
        public double Salary { get; private set; }
        public DateTime CalculationDate { get; private set; }

        public SalaryDataModel(string workerId, DateTime period, double salary)
        {
            Id = Guid.NewGuid().ToString();
            WorkerId = workerId;
            Period = period;
            Salary = salary;
            CalculationDate = DateTime.Now;
        }

        public SalaryDataModel(string id, string workerId, DateTime period, double salary, DateTime calculationDate)
        {
            Id = id;
            WorkerId = workerId;
            Period = period;
            Salary = salary;
            CalculationDate = calculationDate;
        }

        public void Validate()
        {
            if (Id.IsEmpty())
                throw new ValidationException("Field Id is empty");
            if (!Id.IsGuid())
                throw new ValidationException("Id is not a valid GUID");
            if (WorkerId.IsEmpty())
                throw new ValidationException("Field WorkerId is empty");
            if (!WorkerId.IsGuid())
                throw new ValidationException("WorkerId is not a valid GUID");
            if (Salary < 0)
                throw new ValidationException("Salary must be greater than or equal to 0");
        }
    }
}