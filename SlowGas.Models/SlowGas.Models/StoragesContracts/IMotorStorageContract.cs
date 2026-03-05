using SlowGas.Models.DataModels;

namespace SlowGas.Contracts.StoragesContracts
{
    public interface IMotorStorageContract
    {
        List<Motor> GetList(bool onlyActive = true);
        Motor? GetElementById(string id);
        Motor? GetElementByName(string name);
        Motor? GetElementByModelCode(string modelCode);
        void AddElement(Motor motor);
        void UpdateElement(Motor motor);
        void DeleteElement(string id);
    }
}