using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SuperLocker.Api.Models;
using SuperLocker.Core.Command;

namespace SuperLocker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LockController : ControllerBase
    {
        private readonly IBus _bus;

        public LockController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> Lock(LockCommadRequest command)
        {
            await _bus.Publish(new LockCommand(command.LockId, command.UserId));
            return Ok();
        }
    }

}