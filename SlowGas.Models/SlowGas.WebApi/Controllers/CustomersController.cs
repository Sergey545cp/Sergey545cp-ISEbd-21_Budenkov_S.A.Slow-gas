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
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerAdapter _adapter;

        public CustomersController(ICustomerAdapter adapter)
        {
            _adapter = adapter;
        }

        [HttpGet]
        public IActionResult GetAll() => _adapter.GetList().GetResponse(Request, Response);

        [HttpGet("{data}")]
        public IActionResult Get(string data) => _adapter.GetElement(data).GetResponse(Request, Response);

        [HttpPost]
        public IActionResult Create([FromBody] CustomerBindingModel model)
            => _adapter.Create(model).GetResponse(Request, Response);

        [HttpPut]
        public IActionResult Update([FromBody] CustomerBindingModel model)
            => _adapter.Update(model).GetResponse(Request, Response);

        [HttpDelete("{id}")]
        public IActionResult Delete(string id) => _adapter.Delete(id).GetResponse(Request, Response);
    }
}