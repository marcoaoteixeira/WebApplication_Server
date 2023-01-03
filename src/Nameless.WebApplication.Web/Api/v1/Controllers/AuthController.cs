using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Nameless.WebApplication.Domain.Dtos.Common;
using Nameless.WebApplication.Domain.Dtos.v1.Auth;
using Nameless.WebApplication.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Nameless.WebApplication.Api.v1.Controllers {

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
                ? Ok(new { token = response.Token })
                : Unauthorized(new { error = response.Reason });
        }

        #endregion
    }
}
