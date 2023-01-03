using Microsoft.AspNetCore.Http;

namespace Nameless.WebApplication {

    public static class HttpContextExtension {

        #region Public Static Methods

        public static string GetIpAddress(this HttpContext? self) {
            Prevent.Null(self, nameof(self));

            if (self.Request.Headers.ContainsKey("X-Forwarded-For")) {
                return self.Request.Headers["X-Forwarded-For"].ToString();
            }

            return self.Connection.RemoteIpAddress != null
                ? self.Connection.RemoteIpAddress.MapToIPv4().ToString()
                : string.Empty;
        }

        #endregion
    }
}
