using SlowGas.Contracts.BusinessLogicsContracts;
using SlowGas.Contracts.StoragesContracts;
using Microsoft.Extensions.Logging;
using SlowGas.Models.DataModels;

namespace SlowGas.BusinessLogic.Implementations
{
    internal class InvoiceBusinessLogic : IInvoiceBusinessLogic
    {
        private readonly IInvoiceStorageContract _invoiceStorage;
        private readonly IShipmentStorageContract _shipmentStorage;
        private readonly ILogger _logger;

        public InvoiceBusinessLogic(
            IInvoiceStorageContract invoiceStorage,
            IShipmentStorageContract shipmentStorage,
            ILogger logger)
        {
            _invoiceStorage = invoiceStorage;
            _shipmentStorage = shipmentStorage;
            _logger = logger;
        }

        public List<Invoice> GetAllInvoices(DateTime? fromDate = null, DateTime? toDate = null, string? shipmentId = null)
        {
            return new List<Invoice>();
        }

        public Invoice GetInvoiceById(string id)
        {
            return new Invoice();
        }

        public Invoice GetInvoiceByShipmentId(string shipmentId)
        {
            return new Invoice();
        }

        public void CreateInvoice(Invoice invoice)
        {
        }

        public void PayInvoice(string id)
        {
        }
    }
}