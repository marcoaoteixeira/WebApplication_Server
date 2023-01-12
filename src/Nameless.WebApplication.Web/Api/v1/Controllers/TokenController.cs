using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nameless.WebApplication.Api.v1.Controllers {

    [Authorize]
    public sealed class TokenController : ApiControllerBase {

        #region Public Methods

        [HttpPost("refresh")]
        public Task<IActionResult> RefreshAsync([FromBody] string token, CancellationToken cancellationToken = default) {


            throw new NotImplementedException();
        }

        #endregion
    }
}
