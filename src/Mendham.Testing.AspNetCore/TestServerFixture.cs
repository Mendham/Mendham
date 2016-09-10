using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.Features;

namespace Mendham.Testing.AspNetCore
{
    public abstract class TestServerFixture : IServiceProviderFixture
    {
        public TestServerFixture()
        {
            var builder = GetWebHostBuilder();

            Server = new TestServer(builder);
            Client = Server.CreateClient();
            Services = Server.Host.Services;
            ServerFeatures = Server.Host.ServerFeatures;
        }

        protected abstract IWebHostBuilder GetWebHostBuilder();

        public TestServer Server { get; }
        public HttpClient Client { get; }
        public IServiceProvider Services { get; }
        public IFeatureCollection ServerFeatures { get; }

        public virtual void ResetFixture()
        {
        }

        public virtual void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }

    public class TestServerFixture<TStartup> : TestServerFixture where TStartup : class
    {
        protected sealed override IWebHostBuilder GetWebHostBuilder()
        {
            return new WebHostBuilder()
                .ConfigureServices(ServiceConfiguration)
                .UseStartup<TStartup>();
        }

        protected virtual void ServiceConfiguration(IServiceCollection serviceCollection)
        {
        }
    }
}
