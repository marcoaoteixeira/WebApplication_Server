using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nameless.WebApplication.Api.v1.Models;
using Nameless.WebApplication.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Nameless.WebApplication.Api.v1.Controllers
{

    public sealed class AuthController : ApiControllerBase {

        #region Private Read-Only Fields

        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        #endregion

        #region Public Constructors

        public AuthController(IAuthenticationService authenticationService, IMapper mapper) {
            Prevent.Null(authenticationService, nameof(authenticationService));
            Prevent.Null(mapper, nameof(mapper));

            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody, SwaggerRequestBody(Required = true)] AuthenticationInput input, CancellationToken token = default) {
            var request = _mapper.Map<AuthenticationRequest>(input);
            var response = await _authenticationService.AuthenticateAsync(request, token);
            return response.Success
                ? Ok(new { token = response.Token })
                : Unauthorized(new { error = response.Reason });
        }

        #endregion
    }
}
