using SlowGas.Contracts.BusinessLogicsContracts;

using SlowGas.Contracts.StoragesContracts;
using Microsoft.Extensions.Logging;
using SlowGas.Models.DataModels;

namespace SlowGas.BusinessLogic.Implementations
{
    internal class RequestBusinessLogic : IRequestBusinessLogic
    {
        private readonly IRequestStorageContract _requestStorage;
        private readonly ILogger _logger;

        public RequestBusinessLogic(IRequestStorageContract requestStorage, ILogger logger)
        {
            _requestStorage = requestStorage;
            _logger = logger;
        }

        public List<Request> GetAllRequests(DateTime? fromDate = null, DateTime? toDate = null, string? customerId = null)
        {
            return new List<Request>();
        }

        public Request GetRequestById(string id)
        {
            return new Request();
        }

        public void CreateRequest(Request request)
        {
        }

        public void CancelRequest(string id)
        {
        }
    }
}