using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using SlowGas.Contracts.BusinessLogicsContracts;
using SlowGas.Contracts.StoragesContracts;

namespace SlowGas.BusinessLogic.Implementations
{
    public class RequestBusinessLogic : IRequestBusinessLogic
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
            _logger.LogInformation("GetAllRequests called with fromDate={fromDate}, toDate={toDate}, customerId={customerId}",
                fromDate, toDate, customerId);

            if (fromDate.HasValue && toDate.HasValue && fromDate.Value >= toDate.Value)
                throw new IncorrectDatesException(fromDate.Value, toDate.Value);

            var result = _requestStorage.GetList(fromDate, toDate, customerId);
            if (result == null)
                throw new NullListException();

            return result;
        }

        public Request GetRequestById(string id)
        {
            _logger.LogInformation("GetRequestById called with id: {id}", id);

            if (id.IsEmpty())
                throw new ArgumentNullException(nameof(id));

            if (!id.IsGuid())
                throw new ValidationException("Id is not a valid GUID");

            var result = _requestStorage.GetElementById(id);
            if (result == null)
                throw new ElementNotFoundException(id);

            return result;
        }

        public void CreateRequest(Request request)
        {
            _logger.LogInformation("CreateRequest called: {json}", JsonSerializer.Serialize(request));

            ArgumentNullException.ThrowIfNull(request);

            if (request.Date > DateTime.Now)
                throw new ValidationException("Request date cannot be in the future");

            request.Validate();

            _requestStorage.AddElement(request);
        }

        public void CancelRequest(string id)
        {
            _logger.LogInformation("CancelRequest called with id: {id}", id);

            if (id.IsEmpty())
                throw new ArgumentNullException(nameof(id));

            if (!id.IsGuid())
                throw new ValidationException("Id is not a valid GUID");

            _requestStorage.CancelElement(id);
        }
    }
}