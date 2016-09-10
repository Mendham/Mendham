using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.Features;

namespace Mendham.Testing.AspNetCore
{
    /// <summary>
    /// Fixture that creates and manages a <see cref="TestServer"/> to be shared across a test class.
    /// </summary>
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

        /// <summary>
        /// Setup <see cref="IWebHostBuilder"/> to be used by fixture
        /// </summary>
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

    /// <summary>
    /// Fixture that creates and manages a <see cref="TestServer"/> to be shared across a test class. The host
    /// is configured using <typeparamref name="TStartup"/>
    /// </summary>
    public class TestServerFixture<TStartup> : TestServerFixture where TStartup : class
    {
        protected sealed override IWebHostBuilder GetWebHostBuilder()
        {
            return new WebHostBuilder()
                .ConfigureServices(ServiceConfiguration)
                .UseStartup<TStartup>();
        }

        /// <summary>
        /// Extend the service configuration specified by <typeparamref name="TStartup"/>
        /// </summary>
        /// <param name="serviceCollection">ServiceCollection to apply configuration to</param>
        protected virtual void ServiceConfiguration(IServiceCollection serviceCollection)
        {
        }
    }
}
