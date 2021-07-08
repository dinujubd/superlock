using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SuperLocker.Api.Models;
using SuperLocker.Core;
using SuperLocker.Core.Command;
using SuperLocker.Core.Query;
using System.Threading.Tasks;

namespace SuperLocker.Api.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class LockController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IQueryHandler<UnlockActivityQuery, UnlockQueryRespose> _queryHandler;

        public LockController(IBus bus, IQueryHandler<UnlockActivityQuery, UnlockQueryRespose> queryHandler)
        {
            _bus = bus;
            this._queryHandler = queryHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Unlock(UnlockRequest request)
        {
            await _bus.Publish(new UnlockCommand(request.LockId, request.UserId));
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> UnlockActivity([FromQuery] UnlockActivityQuery query)
        {
            var response = await _queryHandler.ExecuteAsync(query);
            return Ok(response);
        }
    }

}