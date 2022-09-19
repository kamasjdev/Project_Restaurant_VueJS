using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Abstractions;
using Restaurant.Application.DTO;

namespace Restaurant.Api.Controllers
{
    public class MailsController : BaseController
    {
        private readonly IMailService _service;

        public MailsController(IMailService service)
        {
            _service = service;
        }

        [HttpPost("send")]
        public async Task<ActionResult> SendAsync(SendOrderOnMailDto command)
        {
            await _service.SendOrderOnMail(command);
            return Ok();
        }
    }
}
