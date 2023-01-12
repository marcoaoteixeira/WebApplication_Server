namespace Nameless.WebApplication.Commands {

    public sealed class Response {

        #region Public Properties

        public object? State { get; }
        public IEnumerable<Error> Errors { get; }
        public string? Message { get; }
        public bool Success => !Errors.Any();

        #endregion

        #region Public Constructors

        public Response(object? state = default, IEnumerable<Error>? errors = default, string? message = default) {
            State = state;
            Errors = errors ?? Array.Empty<Error>();
            Message = message;
        }

        #endregion

        #region Public Static Methods

        public static Response Successful(object? state) => new(state);

        public static Response Failure(IEnumerable<Error> errors, string? message = default) => new(errors: errors, message: message);

        #endregion
    }

    public static class ResponseExtension {

        #region Public Static Methods

        // Syntax sugar
        public static Task<Response> AsTask(this Response self) => Task.FromResult(self);

        #endregion
    }
}
