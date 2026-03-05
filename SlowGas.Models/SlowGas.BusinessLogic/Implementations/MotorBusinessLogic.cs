using SlowGas.Contracts.BusinessLogicsContracts;
using SlowGas.Contracts.StoragesContracts;
using Microsoft.Extensions.Logging;
using SlowGas.Models.DataModels;

namespace SlowGas.BusinessLogic.Implementations
{
    internal class MotorBusinessLogic : IMotorBusinessLogic
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
            return new List<Motor>();
        }

        public Motor GetMotorByData(string data)
        {
            return new Motor();
        }

        public void AddMotor(Motor motor)
        {
        }

        public void UpdateMotor(Motor motor)
        {
        }

        public void DeleteMotor(string id)
        {
        }
    }
}