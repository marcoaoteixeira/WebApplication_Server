using Nameless.WebApplication.Commands;

namespace Nameless.WebApplication.UnitTest.Commands.Fixtures {

    public class SumCommand : Command {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class SumCommandHandler : ICommandHandler<SumCommand> {
        public Task<Response> HandleAsync(SumCommand command, CancellationToken cancellationToken = default) {
            return Response.Successful(command.X + command.Y).AsTask();
        }
    }
}
