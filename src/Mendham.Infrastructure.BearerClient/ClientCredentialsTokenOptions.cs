using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.BearerClient
{
    public class ClientCredentialsTokenOptions : RemoteTokenOptions
    {
        public string ClientSecret { get; set; }
        public List<string> Scopes { get; set; } = new List<string>();
    }
}
