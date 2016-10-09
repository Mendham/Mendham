using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.BearerClient
{
    public class TokenResponseException : Exception
    {
        public string ClientId { get; }
        public TokenResponse TokenReponse { get; }

        public TokenResponseException(TokenResponse tokenResponse, string clientId)
            :base(tokenResponse.Error, tokenResponse.Exception)
        {
            TokenReponse = tokenResponse;
            ClientId = clientId;
        }

        // TODO Override Message
    }
}
