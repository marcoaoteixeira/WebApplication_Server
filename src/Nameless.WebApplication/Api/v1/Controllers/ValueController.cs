using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nameless.WebApplication.Api.v1.Controllers
{

    public sealed class ValueController : ApiControllerBase {

        [HttpGet, AllowAnonymous, Route("get_no_auth")]
        public IActionResult GetWithoutAuthorizationAsync() {
            return Ok(new {
                value = new[] { 1, 2, 3 }
            });
        }

        [HttpGet, Authorize, Route("get_auth")]
        public IActionResult GetWithAuthorizationAsync() {
            return Ok(new {
                value = new[] { 4, 5, 6 }
            });
        }
    }
}
