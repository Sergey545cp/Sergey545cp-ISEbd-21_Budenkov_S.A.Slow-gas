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
    public class ShipmentAdapter : IShipmentAdapter
    {
        private readonly IShipmentStorageContract _storage;
        private readonly IMapper _mapper;
        private readonly ILogger<ShipmentAdapter> _logger;

        public ShipmentAdapter(IShipmentStorageContract storage, IMapper mapper, ILogger<ShipmentAdapter> logger)
        {
            _storage = storage;
            _mapper = mapper;
            _logger = logger;
        }

        public ShipmentOperationResponse GetList(DateTime? fromDate, DateTime? toDate, string? requestId)
        {
            try
            {
                var shipments = _storage.GetList(fromDate, toDate, requestId);
                var viewModels = _mapper.Map<List<ShipmentViewModel>>(shipments);
                return ShipmentOperationResponse.OK(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting shipments");
                return ShipmentOperationResponse.InternalServerError(ex.Message);
            }
        }

        public ShipmentOperationResponse GetElement(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return ShipmentOperationResponse.BadRequest("Id is empty");

                var shipment = _storage.GetElementById(id);
                if (shipment == null)
                    return ShipmentOperationResponse.NotFound($"Shipment not found: {id}");

                return ShipmentOperationResponse.OK(_mapper.Map<ShipmentViewModel>(shipment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting shipment");
                return ShipmentOperationResponse.InternalServerError(ex.Message);
            }
        }

        public ShipmentOperationResponse Create(ShipmentBindingModel model)
        {
            try
            {
                if (model == null)
                    return ShipmentOperationResponse.BadRequest("Model is null");

                var shipment = _mapper.Map<Shipment>(model);
                shipment.Id = Guid.NewGuid().ToString();
                shipment.Date = DateTime.UtcNow;
                _storage.AddElement(shipment);
                return ShipmentOperationResponse.NoContent();
            }
            catch (ElementExistsException ex)
            {
                return ShipmentOperationResponse.BadRequest(ex.Message);
            }
            catch (ElementNotFoundException ex)
            {
                return ShipmentOperationResponse.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating shipment");
                return ShipmentOperationResponse.InternalServerError(ex.Message);
            }
        }

        public ShipmentOperationResponse Cancel(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return ShipmentOperationResponse.BadRequest("Id is empty");

                _storage.CancelElement(id);
                return ShipmentOperationResponse.NoContent();
            }
            catch (ElementNotFoundException ex)
            {
                return ShipmentOperationResponse.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling shipment");
                return ShipmentOperationResponse.InternalServerError(ex.Message);
            }
        }
    }
}