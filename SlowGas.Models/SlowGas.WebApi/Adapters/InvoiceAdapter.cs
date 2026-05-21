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
    public class InvoiceAdapter : IInvoiceAdapter
    {
        private readonly IInvoiceStorageContract _storage;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceAdapter> _logger;

        public InvoiceAdapter(IInvoiceStorageContract storage, IMapper mapper, ILogger<InvoiceAdapter> logger)
        {
            _storage = storage;
            _mapper = mapper;
            _logger = logger;
        }

        public InvoiceOperationResponse GetList(DateTime? fromDate, DateTime? toDate, string? shipmentId)
        {
            try
            {
                var invoices = _storage.GetList(fromDate, toDate, shipmentId);
                var viewModels = _mapper.Map<List<InvoiceViewModel>>(invoices);
                return InvoiceOperationResponse.OK(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting invoices");
                return InvoiceOperationResponse.InternalServerError(ex.Message);
            }
        }

        public InvoiceOperationResponse GetElement(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return InvoiceOperationResponse.BadRequest("Id is empty");

                var invoice = _storage.GetElementById(id);
                if (invoice == null)
                    return InvoiceOperationResponse.NotFound($"Invoice not found: {id}");

                return InvoiceOperationResponse.OK(_mapper.Map<InvoiceViewModel>(invoice));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting invoice");
                return InvoiceOperationResponse.InternalServerError(ex.Message);
            }
        }

        public InvoiceOperationResponse GetByShipmentId(string shipmentId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(shipmentId))
                    return InvoiceOperationResponse.BadRequest("ShipmentId is empty");

                var invoice = _storage.GetElementByShipmentId(shipmentId);
                if (invoice == null)
                    return InvoiceOperationResponse.NotFound($"Invoice not found for shipment: {shipmentId}");

                return InvoiceOperationResponse.OK(_mapper.Map<InvoiceViewModel>(invoice));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting invoice by shipment");
                return InvoiceOperationResponse.InternalServerError(ex.Message);
            }
        }

        public InvoiceOperationResponse Create(InvoiceBindingModel model)
        {
            try
            {
                if (model == null)
                    return InvoiceOperationResponse.BadRequest("Model is null");

                var invoice = _mapper.Map<Invoice>(model);
                invoice.Id = Guid.NewGuid().ToString();
                _storage.AddElement(invoice);
                return InvoiceOperationResponse.NoContent();
            }
            catch (ElementExistsException ex)
            {
                return InvoiceOperationResponse.BadRequest(ex.Message);
            }
            catch (ElementNotFoundException ex)
            {
                return InvoiceOperationResponse.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating invoice");
                return InvoiceOperationResponse.InternalServerError(ex.Message);
            }
        }

        public InvoiceOperationResponse Pay(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return InvoiceOperationResponse.BadRequest("Id is empty");

                _storage.PayInvoice(id);
                return InvoiceOperationResponse.NoContent();
            }
            catch (ElementNotFoundException ex)
            {
                return InvoiceOperationResponse.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error paying invoice");
                return InvoiceOperationResponse.InternalServerError(ex.Message);
            }
        }
    }
}