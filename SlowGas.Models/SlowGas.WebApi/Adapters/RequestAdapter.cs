using AutoMapper;
using SlowGas.Contracts.StoragesContracts;
using SlowGas.Models.BindingModels;
using SlowGas.Models.DataModels;
using SlowGas.Models.Exceptions;
using SlowGas.Models.StoragesContracts;
using SlowGas.Models.ViewModels;
using SlowGas.WebApi.Infrastructure;

namespace SlowGas.WebApi.Adapters
{
    public class RequestAdapter : IRequestAdapter
    {
        private readonly IRequestStorageContract _storage;
        private readonly IMapper _mapper;
        private readonly ILogger<RequestAdapter> _logger;

        public RequestAdapter(IRequestStorageContract storage, IMapper mapper, ILogger<RequestAdapter> logger)
        {
            _storage = storage;
            _mapper = mapper;
            _logger = logger;
        }

        public RequestOperationResponse GetList(DateTime? fromDate, DateTime? toDate, string? customerId)
        {
            try
            {
                var requests = _storage.GetList(fromDate, toDate, customerId);
                var viewModels = _mapper.Map<List<RequestViewModel>>(requests);
                return RequestOperationResponse.OK(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting requests");
                return RequestOperationResponse.InternalServerError(ex.Message);
            }
        }

        public RequestOperationResponse GetElement(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return RequestOperationResponse.BadRequest("Id is empty");

                var request = _storage.GetElementById(id);
                if (request == null)
                    return RequestOperationResponse.NotFound($"Request not found: {id}");

                return RequestOperationResponse.OK(_mapper.Map<RequestViewModel>(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting request");
                return RequestOperationResponse.InternalServerError(ex.Message);
            }
        }

        public RequestOperationResponse Create(RequestBindingModel model)
        {
            try
            {
                if (model == null)
                    return RequestOperationResponse.BadRequest("Model is null");

                var request = _mapper.Map<Request>(model);
                request.Id = Guid.NewGuid().ToString();
                request.Date = DateTime.UtcNow;
                _storage.AddElement(request);
                return RequestOperationResponse.NoContent();
            }
            catch (ElementExistsException ex)
            {
                return RequestOperationResponse.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating request");
                return RequestOperationResponse.InternalServerError(ex.Message);
            }
        }

        public RequestOperationResponse Cancel(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return RequestOperationResponse.BadRequest("Id is empty");

                _storage.CancelElement(id);
                return RequestOperationResponse.NoContent();
            }
            catch (ElementNotFoundException ex)
            {
                return RequestOperationResponse.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling request");
                return RequestOperationResponse.InternalServerError(ex.Message);
            }
        }
    }
}