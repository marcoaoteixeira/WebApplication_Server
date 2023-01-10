using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nameless.WebApplication.Api.Auth.v1.Models.Input;
using Nameless.WebApplication.Services;
using Nameless.WebApplication.Services.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Nameless.WebApplication.Api.Auth.v1.Controllers {

    [Authorize]
    public sealed class AuthController : ApiControllerBase {

        #region Private Read-Only Fields

        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        private readonly IValidator<AuthenticationInput> _authenticationInputValidator;

        #endregion

        #region Public Constructors

        public AuthController(IAuthenticationService authenticationService, IMapper mapper, IValidator<AuthenticationInput> authenticationInputValidator) {
            Prevent.Null(authenticationService, nameof(authenticationService));
            Prevent.Null(mapper, nameof(mapper));
            Prevent.Null(authenticationInputValidator, nameof(authenticationInputValidator));

            _authenticationService = authenticationService;
            _mapper = mapper;
            _authenticationInputValidator = authenticationInputValidator;
        }

        #endregion

        #region Public Methods

        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PostAsync([FromBody, SwaggerRequestBody(Required = true)] AuthenticationInput input, CancellationToken cancellationToken = default) {
            var validationResult = await _authenticationInputValidator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid) {
                validationResult.PushIntoModelState(ModelState);

                return BadRequest(ModelState);
            }

            var request = _mapper.Map<AuthenticationRequest>(input);
            var response = await _authenticationService.AuthenticateAsync(request, cancellationToken);

            return response.Success
                ? Ok(response)
                : Unauthorized(new { error = response.Reason });
        }

        #endregion
    }
}
