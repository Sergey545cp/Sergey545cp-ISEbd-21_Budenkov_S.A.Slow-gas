using SlowGas.Models.DataModels;

namespace SlowGas.Contracts.BusinessLogicsContracts
{
    public interface IRequestBusinessLogic
    {
        List<Request> GetAllRequests(DateTime? fromDate = null, DateTime? toDate = null, string? customerId = null);
        Request GetRequestById(string id);
        void CreateRequest(Request request);
        void CancelRequest(string id);
    }
}