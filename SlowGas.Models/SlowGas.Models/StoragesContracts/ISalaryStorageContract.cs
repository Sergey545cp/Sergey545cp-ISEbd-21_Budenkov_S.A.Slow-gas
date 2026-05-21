using SlowGas.Models.DataModels;

namespace SlowGas.Models.StoragesContracts
{
    public interface ISalaryStorageContract
    {
        List<SalaryDataModel> GetList(DateTime fromDate, DateTime toDate);
        List<SalaryDataModel> GetList(DateTime fromDate, DateTime toDate, string workerId);
        SalaryDataModel GetElementById(string id);
        void AddElement(SalaryDataModel model);
        void UpdateElement(SalaryDataModel model);
        void DeleteElement(string id);
    }
}