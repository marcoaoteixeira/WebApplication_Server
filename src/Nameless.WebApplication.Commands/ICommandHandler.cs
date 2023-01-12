namespace Nameless.WebApplication.Commands {

    public interface ICommandHandler<in TCommand>
        where TCommand : Command {

        #region Methods

        Task<Response> HandleAsync(TCommand command, CancellationToken cancellationToken = default);

        #endregion
    }
}
