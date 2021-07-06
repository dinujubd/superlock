using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SuperLocker.Api.Models;
using SuperLocker.Core;
using SuperLocker.Core.Command;
using SuperLocker.Core.Query;

namespace SuperLocker.Api.Controllers
{
    [ApiController]
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
        public async Task<IActionResult> Unlock(LockCommadRequest command)
        {
            await _bus.Publish(new UnlockCommand(command.LockId, command.UserId));
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> UnlockActivity(UnlockActivityQuery query)
        {
            var response = await _queryHandler.ExecuteAsync(query);
            return Ok(response);
        }
    }

}