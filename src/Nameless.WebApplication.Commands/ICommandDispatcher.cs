namespace Nameless.WebApplication.Commands
{

    public interface ICommandDispatcher
    {

        #region Methods

        Task<Response> DispatchAsync(Command command, CancellationToken cancellationToken = default);

        #endregion
    }
}
