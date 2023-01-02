using Nameless.WebApplication;

namespace Nameless.WebApplication.Domain
{
    public abstract class Response<TType> where TType : class {

        #region Public Properties

        public TType Value { get; } = null!;
        public ValidationError[] ValidationErrors { get; } = null!;
        public bool Success => !ValidationErrors.Any();

        #endregion

        #region Public Constructors

        public Response(TType value, params ValidationError[] validationErrors) {
            Prevent.Null(value, nameof(value));
            Prevent.Null(validationErrors, nameof(validationErrors));

            Value = value;
            ValidationErrors = validationErrors;
        }

        #endregion
    }
}
