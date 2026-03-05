using SlowGas.Models.DataModels;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using SlowGas.Contracts.BusinessLogicsContracts;
using SlowGas.Contracts.StoragesContracts;

namespace SlowGas.BusinessLogic.Implementations
{
    public class MotorBusinessLogic : IMotorBusinessLogic
    {
        private readonly IMotorStorageContract _motorStorage;
        private readonly ILogger _logger;

        public MotorBusinessLogic(IMotorStorageContract motorStorage, ILogger logger)
        {
            _motorStorage = motorStorage;
            _logger = logger;
        }

        public List<Motor> GetAllMotors(bool onlyActive = true)
        {
            _logger.LogInformation("GetAllMotors called with onlyActive: {onlyActive}", onlyActive);
            var result = _motorStorage.GetList(onlyActive);
            if (result == null)
                throw new NullListException();
            return result;
        }

        public Motor GetMotorByData(string data)
        {
            _logger.LogInformation("GetMotorByData called with data: {data}", data);

            if (data.IsEmpty())
                throw new ArgumentNullException(nameof(data));

            if (data.IsGuid())
            {
                var result = _motorStorage.GetElementById(data);
                if (result == null)
                    throw new ElementNotFoundException(data);
                return result;
            }

            var resultByName = _motorStorage.GetElementByName(data);
            if (resultByName != null)
                return resultByName;

            var resultByCode = _motorStorage.GetElementByModelCode(data);
            if (resultByCode == null)
                throw new ElementNotFoundException(data);

            return resultByCode;
        }

        public void AddMotor(Motor motor)
        {
            _logger.LogInformation("AddMotor called: {json}", JsonSerializer.Serialize(motor));
            ArgumentNullException.ThrowIfNull(motor);
            motor.Validate();
            _motorStorage.AddElement(motor);
        }

        public void UpdateMotor(Motor motor)
        {
            _logger.LogInformation("UpdateMotor called: {json}", JsonSerializer.Serialize(motor));
            ArgumentNullException.ThrowIfNull(motor);
            motor.Validate();
            _motorStorage.UpdateElement(motor);
        }

        public void DeleteMotor(string id)
        {
            _logger.LogInformation("DeleteMotor called with id: {id}", id);

            if (id.IsEmpty())
                throw new ArgumentNullException(nameof(id));

            if (!id.IsGuid())
                throw new ValidationException("Id is not a valid GUID");

            _motorStorage.DeleteElement(id);
        }
    }
}