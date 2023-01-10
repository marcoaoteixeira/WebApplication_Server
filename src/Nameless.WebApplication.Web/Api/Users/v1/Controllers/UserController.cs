using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Nameless.WebApplication.Api.Users.v1.Models.Input;
using Nameless.WebApplication.Api.Users.v1.Models.Output;
using Nameless.WebApplication.Entities;
using Nameless.WebApplication.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Nameless.WebApplication.Api.Users.v1.Controllers {

    public sealed class UserController : ApiControllerBase {

        #region Private Read-Only Fields

        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserInput> _createUserInputValidator;
        private readonly IValidator<UpdateUserInput> _updateUserInputValidator;
        private readonly IValidator<AddClaimInput> _addClaimInputValidator;

        #endregion

        #region Public Constructors

        public UserController(
            IUserManager userManager,
            IMapper mapper,
            IValidator<CreateUserInput> createUserInputValidator,
            IValidator<UpdateUserInput> updateUserInputValidator,
            IValidator<AddClaimInput> addClaimInputValidator
        ) {
            Prevent.Null(userManager, nameof(userManager));
            Prevent.Null(mapper, nameof(mapper));
            Prevent.Null(createUserInputValidator, nameof(createUserInputValidator));
            Prevent.Null(updateUserInputValidator, nameof(updateUserInputValidator));
            Prevent.Null(addClaimInputValidator, nameof(addClaimInputValidator));

            _userManager = userManager;
            _mapper = mapper;
            _createUserInputValidator = createUserInputValidator;
            _updateUserInputValidator = updateUserInputValidator;
            _addClaimInputValidator = addClaimInputValidator;
        }

        #endregion

        #region Public Methods

        [HttpGet, Route("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken = default) {
            var user = await _userManager.GetByIDAsync(id, cancellationToken);
            var result = _mapper.Map<UserOutput>(user);

            return Ok(result);
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody, SwaggerRequestBody(Required = true)] CreateUserInput input, CancellationToken cancellationToken = default) {
            var validationResult = await _createUserInputValidator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid) {
                validationResult.PushIntoModelState(ModelState);

                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(input);
            await _userManager.CreateAsync(user, cancellationToken);
            var output = _mapper.Map<CreateUserOutput>(user);

            return Ok(output);
        }

        [HttpPut]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromQuery] Guid id, [FromBody, SwaggerRequestBody(Required = true)] UpdateUserInput input, CancellationToken cancellationToken = default) {
            var validationResult = await _updateUserInputValidator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid) {
                validationResult.PushIntoModelState(ModelState);

                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(input);
            await _userManager.UpdateAsync(id, user, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromQuery] Guid userID, CancellationToken cancellationToken = default) {
            await _userManager.DeleteAsync(userID, cancellationToken);

            return NoContent();
        }

        [HttpPost, Route("{id}/claims")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddClaimAsync([FromQuery] Guid id, [FromBody, SwaggerRequestBody(Required = true)] AddClaimInput input, CancellationToken cancellationToken = default) {
            var validationResult = await _addClaimInputValidator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid) {
                validationResult.PushIntoModelState(ModelState);

                return BadRequest(ModelState);
            }

            var claim = _mapper.Map<Claim>(input);
            await _userManager.AddClaimsAsync(id, new[] { claim }, cancellationToken);

            return Ok();
        }

        [HttpDelete, Route("{id}/claims")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveClaimsAsync([FromQuery] Guid id, [FromBody, SwaggerRequestBody(Required = true)] RemoveClaimInput input, CancellationToken cancellationToken = default) {
            var claim = _mapper.Map<Claim>(input);

            await _userManager.RemoveClaimsAsync(id, new[] { claim }, cancellationToken);

            return NoContent();
        }

        #endregion
    }
}
