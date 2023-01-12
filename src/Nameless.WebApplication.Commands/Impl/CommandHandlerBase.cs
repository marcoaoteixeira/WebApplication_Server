using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Commands.Impl {

    public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand>
        where TCommand : Command {

        #region Private Fields

        private ILogger _logger = default!;

        #endregion

        #region Protected Properties

        protected ApplicationDbContext DbContext { get; }
        protected IMapper Mapper { get; }
        protected IValidator<TCommand>? Validator { get; }
        protected ILogger Logger {
            get { return _logger ?? NullLogger.Instance; }
            set { _logger = value ?? NullLogger.Instance; }
        }

        #endregion

        #region Protected Constructors

        protected CommandHandlerBase(ApplicationDbContext dbContext, IMapper mapper, IValidator<TCommand>? validator = default) {
            Prevent.Null(dbContext, nameof(dbContext));
            Prevent.Null(mapper, nameof(mapper));

            DbContext = dbContext;
            Mapper = mapper;
            Validator = validator;
        }

        #endregion

        #region Protected Methods

        public abstract Task<Response> InnerHandleAsync(TCommand command, CancellationToken cancellationToken = default);

        #endregion

        #region ICommandHandler<TCommand, TResponse, TOutput> Members

        public async Task<Response> HandleAsync(TCommand command, CancellationToken cancellationToken = default) {
            if (Validator != null) {
                var validation = await Validator.ValidateAsync(command, cancellationToken);
                if (!validation.IsValid) {
                    return Mapper.Map<Response>(validation);
                }
            }

            return await InnerHandleAsync(command, cancellationToken);
        }

        #endregion
    }
}
