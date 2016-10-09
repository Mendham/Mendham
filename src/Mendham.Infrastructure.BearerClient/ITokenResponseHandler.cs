using IdentityModel.Client;
using System;

namespace Mendham.Infrastructure.BearerClient
{
    public interface ITokenResponseHandler
    {
        void EnsureTokenResponseSuccessful(TokenResponse tokenResponse, Func<bool> tokenResponseCancellationRequested);
        void SetRemoteTokenOptions(RemoteTokenOptions options);
    }
}
