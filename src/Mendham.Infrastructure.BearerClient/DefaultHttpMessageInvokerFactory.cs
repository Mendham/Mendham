using System.Net.Http;

namespace Mendham.Infrastructure.BearerClient
{
    public class DefaultHttpMessageInvokerFactory : IHttpMessageInvokerFactory
    {
        public HttpMessageInvoker CreateHttpMessageInvoker()
        {
            var handler = new HttpClientHandler();
            return new HttpMessageInvoker(handler, true);
        }
    }
}
