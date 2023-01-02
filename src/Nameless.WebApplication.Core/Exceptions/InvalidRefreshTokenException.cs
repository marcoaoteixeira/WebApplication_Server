using System.Runtime.Serialization;

namespace Nameless.WebApplication {


    [Serializable]
    public class InvalidRefreshTokenException : Exception {
        #region Public Constructors

        public InvalidRefreshTokenException()
            : this("Invalid refresh token") { }
        public InvalidRefreshTokenException(string message)
            : base(message) { }
        public InvalidRefreshTokenException(string message, Exception inner)
            : base(message, inner) { }

        #endregion

        #region Protected Constructors

        protected InvalidRefreshTokenException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
