using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Nameless.WebApplication.Domain.Dtos.Common;
using Nameless.WebApplication.Domain.Dtos.v1.Users;
using Nameless.WebApplication.Entities;
using Nameless.WebApplication.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Nameless.WebApplication.Api.v1.Controllers {

    public sealed class UserController : ApiControllerBase {

        #region Private Read-Only Fields

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserInput> _createUserInputValidator;

        #endregion

        #region Public Constructors

        public UserController(IUserService userService, IMapper mapper, IValidator<CreateUserInput> createUserInputValidator) {
            Prevent.Null(userService, nameof(userService));
            Prevent.Null(mapper, nameof(mapper));
            Prevent.Null(createUserInputValidator, nameof(createUserInputValidator));

            _userService = userService;
            _mapper = mapper;
            _createUserInputValidator = createUserInputValidator;
        }

        #endregion

        #region Public Methods

        [HttpGet, Route("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken = default) {
            var user = await _userService.GetByIDAsync(id, cancellationToken);
            var result = _mapper.Map<UserOutput>(user);

            return Ok(result);
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync([FromQuery] PageRequest request, CancellationToken cancellationToken = default) {
            var user = await _userService.SearchAsync(request.Index, request.Size, request.SearchTerm, request.OrderBy, cancellationToken);
            var result = _mapper.Map<UserOutput[]>(user);

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
            var result = await _userService.CreateAsync(user, cancellationToken);
            var output = _mapper.Map<CreateUserOutput>(result);

            return Ok(output);
        }

        #endregion
    }
}
