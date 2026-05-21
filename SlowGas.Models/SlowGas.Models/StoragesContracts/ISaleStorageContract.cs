using SlowGas.Models.DataModels;

namespace SlowGas.Models.StoragesContracts
{
    public interface ISaleStorageContract
    {
        List<SaleDataModel> GetList(DateTime fromDate, DateTime toDate);
        List<SaleDataModel> GetList(DateTime fromDate, DateTime toDate, string workerId, string customerId, bool? isReturned);
        SaleDataModel GetElementById(string id);
        void AddElement(SaleDataModel model);
        void UpdateElement(SaleDataModel model);
        void DeleteElement(string id);
    }
}