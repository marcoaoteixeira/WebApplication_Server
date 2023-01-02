using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nameless.WebApplication.Api.v1.Controllers
{

    public class HomeController : ApiControllerBase {

        #region Methods

        [HttpGet]
        public IActionResult Get() {
            return Ok(new[] { 1, 2, 3 });
        }

        [Authorize]
        [HttpGet, Route("/list")]
        public IActionResult List() {
            return Ok(new[] { 1, 2, 3 });
        }

        #endregion
    }
}
