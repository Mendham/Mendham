using Mendham.Testing.Http.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentAssertions
{
    public static class HttpAssertionExtensions
    {
        public static HttpResponseMessageAssestions Should(this HttpResponseMessage responseMessage)
        {
            return new HttpResponseMessageAssestions(responseMessage);
        }

        public static HttpContentAssertions Should(this HttpContent httpContent)
        {
            return new HttpContentAssertions(httpContent);
        }
    }
}
