using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlowGas.Models.BindingModels;
using SlowGas.WebApi.Adapters;
using SlowGas.WebApi.Infrastructure;

namespace SlowGas.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceAdapter _adapter;

        public InvoicesController(IInvoiceAdapter adapter)
        {
            _adapter = adapter;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? shipmentId)
            => _adapter.GetList(fromDate, toDate, shipmentId).GetResponse(Request, Response);

        [HttpGet("{id}")]
        public IActionResult Get(string id) => _adapter.GetElement(id).GetResponse(Request, Response);

        [HttpGet("byshipment/{shipmentId}")]
        public IActionResult GetByShipment(string shipmentId) => _adapter.GetByShipmentId(shipmentId).GetResponse(Request, Response);

        [HttpPost]
        public IActionResult Create([FromBody] InvoiceBindingModel model)
            => _adapter.Create(model).GetResponse(Request, Response);

        [HttpPatch("{id}/pay")]
        public IActionResult Pay(string id) => _adapter.Pay(id).GetResponse(Request, Response);
    }
}