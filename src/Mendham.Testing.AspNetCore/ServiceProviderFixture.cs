using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Mendham.Testing.AspNetCore
{
    public abstract class ServiceProviderFixture : IServiceProviderFixture
    {
        public IServiceProvider Services { get; }

        public ServiceProviderFixture()
        {
            var serviceCollection = new ServiceCollection();

            ServiceConfiguration(serviceCollection);

            Services = serviceCollection.BuildServiceProvider();
        }

        protected abstract void ServiceConfiguration(IServiceCollection serviceCollection);

        public virtual void ResetFixture()
        {
        }

        public virtual void Dispose()
        {
            (Services as IDisposable)?.Dispose();
        }
    }

    public class ServiceProviderFixture<TStartup> : IServiceProviderFixture where TStartup : class
    {
        private IWebHost _webHost;

        public ServiceProviderFixture()
        {
            _webHost = new WebHostBuilder()
                .ConfigureServices(ServiceConfiguration)
                .UseStartup<TStartup>()
                .UseServer(new NoopServer())
                .Build();

            Services = _webHost.Services;
        }

        public IServiceProvider Services { get; }

        protected virtual void ServiceConfiguration(IServiceCollection serviceCollection)
        {
        }

        public virtual void ResetFixture()
        {
        }

        public virtual void Dispose()
        {
            _webHost?.Dispose();
        }

        private class NoopServer : IServer
        {
            public IFeatureCollection Features { get; }

            public void Dispose()
            {
            }

            public void Start<TContext>(IHttpApplication<TContext> application)
            {
                throw new NotImplementedException($"Cannot start the host from {nameof(ServiceProviderFixture<TStartup>)}, use {nameof(TestServerFixture<TStartup>)} instead.");
            }
        }
    }
}
