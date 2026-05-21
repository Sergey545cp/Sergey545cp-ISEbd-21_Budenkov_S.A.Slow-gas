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
    public class RequestsController : ControllerBase
    {
        private readonly IRequestAdapter _adapter;

        public RequestsController(IRequestAdapter adapter)
        {
            _adapter = adapter;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? customerId)
            => _adapter.GetList(fromDate, toDate, customerId).GetResponse(Request, Response);

        [HttpGet("{id}")]
        public IActionResult Get(string id) => _adapter.GetElement(id).GetResponse(Request, Response);

        [HttpPost]
        public IActionResult Create([FromBody] RequestBindingModel model)
            => _adapter.Create(model).GetResponse(Request, Response);

        [HttpDelete("{id}")]
        public IActionResult Cancel(string id) => _adapter.Cancel(id).GetResponse(Request, Response);
    }
}