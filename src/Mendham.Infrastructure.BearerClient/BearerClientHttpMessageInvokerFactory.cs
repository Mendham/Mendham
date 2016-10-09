using System.Net.Http;

namespace Mendham.Infrastructure.BearerClient
{
    public class BearerClientHttpMessageInvokerFactory : IHttpMessageInvokerFactory
    {
        private readonly BearerClientOptions _options;

        public BearerClientHttpMessageInvokerFactory(BearerClientOptions options)
        {
            _options = options;
        }

        public HttpMessageInvoker CreateHttpMessageInvoker()
        {
            var handler = _options.InnerMessageHandler ?? new HttpClientHandler();
            var disposeHandler = _options.InnerMessageHandler == null || _options.DisposeInnerMessageHandler;
            return new HttpMessageInvoker(handler, disposeHandler);
        }
    }
}
