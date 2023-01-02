using System.Diagnostics.CodeAnalysis;

namespace Nameless.WebApplication.Services {

    public interface IJsonWebTokenService {

        #region Methods

        Task<string> GenerateTokenAsync(string value, CancellationToken cancellationToken = default);

        Task<bool> ValidateTokenAsync(string token, [NotNullWhen(true)] out string? value, CancellationToken cancellationToken = default);

        #endregion
    }
}
