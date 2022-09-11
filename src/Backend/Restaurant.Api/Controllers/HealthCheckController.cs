using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class HealthCheckController : ControllerBase
    {
        private readonly AppOptions _appOptions;

        public HealthCheckController(IOptionsMonitor<AppOptions> appOptions)
        {
            _appOptions = appOptions.CurrentValue;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok(_appOptions.Name);
        }
    }
}
