using SlowGas.Models.DataModels;

namespace SlowGas.Contracts.StoragesContracts
{
    public interface IRequestStorageContract
    {
        List<Request> GetList(DateTime? fromDate = null, DateTime? toDate = null, string? customerId = null);
        Request? GetElementById(string id);
        void AddElement(Request request);
        void CancelElement(string id);
    }
}