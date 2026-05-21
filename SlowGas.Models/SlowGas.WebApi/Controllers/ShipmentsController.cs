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
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentAdapter _adapter;

        public ShipmentsController(IShipmentAdapter adapter)
        {
            _adapter = adapter;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? requestId)
            => _adapter.GetList(fromDate, toDate, requestId).GetResponse(Request, Response);

        [HttpGet("{id}")]
        public IActionResult Get(string id) => _adapter.GetElement(id).GetResponse(Request, Response);

        [HttpPost]
        public IActionResult Create([FromBody] ShipmentBindingModel model)
            => _adapter.Create(model).GetResponse(Request, Response);

        [HttpDelete("{id}")]
        public IActionResult Cancel(string id) => _adapter.Cancel(id).GetResponse(Request, Response);
    }
}