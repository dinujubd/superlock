using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperLocker.Api.Models;
using SuperLocker.Core;
using SuperLocker.Core.Query;
using System.Threading.Tasks;

namespace SuperLocker.Api.Controllers
{
    [Authorize(Roles = "admin,appuser")]
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class ActitiyQueryController : ControllerBase
    {
     
        private readonly IQueryHandler<UnlockActivityQuery, UnlockQueryRespose> _queryHandler;
        private readonly AppUser _user;

        public ActitiyQueryController(IQueryHandler<UnlockActivityQuery, UnlockQueryRespose> queryHandler, AppUser user)
        {
            _user = user;
            this._queryHandler = queryHandler;
        }


        [HttpGet]
        public async Task<IActionResult> UnlockActivity()
        {
            var response = await _queryHandler.ExecuteAsync(new UnlockActivityQuery { UserId = _user.UserId });
            return response.IsValid ? Ok(response) : BadRequest(response);
        }
    }

}