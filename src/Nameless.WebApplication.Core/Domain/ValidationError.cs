using Nameless.WebApplication;

namespace Nameless.WebApplication.Domain {

    public sealed class ValidationError {

        #region Public Properties

        public string Name { get; } = null!;
        public string[] Messages { get; } = null!;

        #endregion

        #region Public Constructors

        public ValidationError(string name, params string[] messages) {
            Prevent.Null(name, nameof(name));
            Prevent.Null(messages, nameof(messages));

            Name = name;
            Messages = messages;
        }

        #endregion
    }
}
