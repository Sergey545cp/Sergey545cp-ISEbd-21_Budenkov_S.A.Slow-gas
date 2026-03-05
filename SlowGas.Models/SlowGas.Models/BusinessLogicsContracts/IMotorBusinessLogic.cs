using SlowGas.Models.DataModels;

namespace SlowGas.Contracts.BusinessLogicsContracts
{
    public interface IMotorBusinessLogic
    {
        List<Motor> GetAllMotors(bool onlyActive = true);
        Motor GetMotorByData(string data);
        void AddMotor(Motor motor);
        void UpdateMotor(Motor motor);
        void DeleteMotor(string id);
    }
}