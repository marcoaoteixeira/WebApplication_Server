using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nameless.WebApplication.Domain.v1.Users.Models.Input;
using Nameless.WebApplication.Domain.v1.Users.Models.Output;
using Nameless.WebApplication.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Nameless.WebApplication.Api.v1.Controllers {

    public sealed class UserController : ApiControllerBase {

        #region Private Read-Only Fields

        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserInput> _createUserInputValidator;
        private readonly IValidator<AddClaimInput> _addClaimInputValidator;
        private readonly IValidator<RemoveClaimInput> _removeClaimInputValidator;

        #endregion

        #region Public Constructors

        public UserController(
            UserManager<User> userManager,
            IMapper mapper,
            IValidator<CreateUserInput> createUserInputValidator,
            IValidator<AddClaimInput> addClaimInputValidator,
            IValidator<RemoveClaimInput> removeClaimInputValidator
        ) {
            Prevent.Null(userManager, nameof(userManager));
            Prevent.Null(mapper, nameof(mapper));
            Prevent.Null(createUserInputValidator, nameof(createUserInputValidator));
            Prevent.Null(addClaimInputValidator, nameof(addClaimInputValidator));
            Prevent.Null(removeClaimInputValidator, nameof(removeClaimInputValidator));

            _userManager = userManager;
            _mapper = mapper;
            _createUserInputValidator = createUserInputValidator;
            _addClaimInputValidator = addClaimInputValidator;
            _removeClaimInputValidator = removeClaimInputValidator;
        }

        #endregion

        #region Public Methods

        [HttpGet, Route("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(Guid id) {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == default) { return NotFound(); }

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
            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded) { result.PushIntoModelState(ModelState); }

            return result.Succeeded
                ? Ok(_mapper.Map<CreateUserOutput>(user))
                : BadRequest(ModelState);
        }

        [HttpDelete]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync([FromQuery] Guid userId) {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == default) { return NotFound(); }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded) { result.PushIntoModelState(ModelState); }

            return result.Succeeded
                ? NoContent()
                : BadRequest(ModelState);
        }

        [HttpPost, Route("{id}/claims")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddClaimAsync([FromQuery] Guid id, [FromBody, SwaggerRequestBody(Required = true)] AddClaimInput input, CancellationToken cancellationToken = default) {
            var validationResult = await _addClaimInputValidator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid) {
                validationResult.PushIntoModelState(ModelState);

                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == default) { return NotFound(); }

            var claim = _mapper.Map<Claim>(input);
            var result = await _userManager.AddClaimAsync(user, claim);

            if (!result.Succeeded) { result.PushIntoModelState(ModelState); }

            return result.Succeeded
                ? Ok(_mapper.Map<AddClaimOutput>(claim))
                : BadRequest(ModelState);
        }

        [HttpDelete, Route("{id}/claims")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveClaimsAsync([FromQuery] Guid id, [FromBody, SwaggerRequestBody(Required = true)] RemoveClaimInput input, CancellationToken cancellationToken = default) {
            var validationResult = await _removeClaimInputValidator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid) {
                validationResult.PushIntoModelState(ModelState);

                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == default) { return NotFound(); }

            var claim = _mapper.Map<Claim>(input);
            var result = await _userManager.RemoveClaimAsync(user, claim);

            if (!result.Succeeded) { result.PushIntoModelState(ModelState); }

            return result.Succeeded
                ? NoContent()
                : BadRequest(ModelState);
        }

        #endregion
    }
}
