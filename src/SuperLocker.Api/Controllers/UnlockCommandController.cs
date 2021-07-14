using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperLocker.Api.Models;
using SuperLocker.Core.Command;
using System.Threading.Tasks;

namespace SuperLocker.Api.Controllers
{
    [Authorize(Roles = "admin,appuser")]
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class UnlockCommandController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly AppUser _user;

        public UnlockCommandController(IBus bus, AppUser user)
        {
            _bus = bus;
            _user = user;
        }

        [HttpPost]
        public async Task<IActionResult> Unlock(UnlockRequest request)
        {
            try
            {
                await _bus.Send(new UnlockCommand(request.LockId, _user.UserId));
                return Ok();
            }
            catch
            {
                return BadRequest();
            }   
        }

    }

}
