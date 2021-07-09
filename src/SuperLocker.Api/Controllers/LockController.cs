using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperLocker.Api.Models;
using SuperLocker.Core;
using SuperLocker.Core.Command;
using SuperLocker.Core.Query;
using System.Threading.Tasks;

namespace SuperLocker.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class LockController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IQueryHandler<UnlockActivityQuery, UnlockQueryRespose> _queryHandler;
        private readonly AppUser _user;

        public LockController(IBus bus, IQueryHandler<UnlockActivityQuery, UnlockQueryRespose> queryHandler, AppUser user)
        {
            _bus = bus;
            _user = user;
            this._queryHandler = queryHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Unlock(UnlockRequest request)
        {
            await _bus.Publish(new UnlockCommand(request.LockId, _user.UserId));
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> UnlockActivity()
        {
            var response = await _queryHandler.ExecuteAsync(new UnlockActivityQuery { UserId = _user.UserId });
            return response.IsValid ? Ok(response) : BadRequest(response);
        }
    }

}