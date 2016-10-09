using System.Net.Http;

namespace Mendham.Infrastructure.BearerClient
{
    public interface IHttpMessageInvokerFactory
    {
        HttpMessageInvoker CreateHttpMessageInvoker();
    }
}
