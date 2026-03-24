using SlowGas.Models.DataModels;

namespace SlowGas.Models.StoragesContracts
{
    public interface IMotorComponentsStorageContract
    {
        List<ComponentDataModel> GetComponentsForMotor(string motorModelId);
        List<MotorModelDataModel> GetMotorsForComponent(string componentId);
        void AddComponentToMotor(MotorComponentsDataModel motorComponentsDataModel);
        void RemoveComponentFromMotor(string motorModelId, string componentId);
        void UpdateQuantity(string motorModelId, string componentId, int newQuantity);
    }
}