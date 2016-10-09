using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.BearerClient
{
    public class BearerClientOptions
    {

        public HttpMessageHandler InnerMessageHandler { get; set; }
        public bool DisposeInnerMessageHandler { get; set; } = true;

        public TimeSpan? MessageTimeout { get; set; }
    }
}
