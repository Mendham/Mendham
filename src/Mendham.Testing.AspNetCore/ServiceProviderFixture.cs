using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mendham.Testing.AspNetCore
{
    /// <summary>
    /// Fixtured used to manage a <see cref="IServiceProvider"/> shared across a test class.
    /// </summary>
    public abstract class ServiceProviderFixture : IServiceProviderFixture
    {
        public IServiceProvider Services { get; }

        public ServiceProviderFixture()
        {
            var serviceCollection = new ServiceCollection();

            ServiceConfiguration(serviceCollection);

            Services = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Setup <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="serviceCollection">ServiceCollection to apply configuration to</param>
        protected abstract void ServiceConfiguration(IServiceCollection serviceCollection);

        public virtual void ResetFixture()
        {
        }

        public virtual void Dispose()
        {
            (Services as IDisposable)?.Dispose();
        }
    }

    /// <summary>
    /// Fixtured used to manage a <see cref="IServiceProvider"/> shared across a test class. A <typeparamref name="TStartup"/>
    /// class provides default service configuration. Those values can be extended by overriding ServiceConfiguration
    /// </summary>
    /// <typeparam name="TStartup">Startup type used for service configuration</typeparam>
    public class ServiceProviderFixture<TStartup> : IServiceProviderFixture where TStartup : class
    {
        private IWebHost _webHost;

        public ServiceProviderFixture()
        {
            // WebHost is just built to apply TStartup harvest IServiceProvider 
            _webHost = new WebHostBuilder()
                .ConfigureServices(ServiceConfiguration)
                .UseStartup<TStartup>()
                .UseServer(new NoopServer())
                .Build();

            Services = _webHost.Services;
        }

        /// <summary>
        /// <see cref="IServiceProvider"/> that can be used to retrieve objects
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Extend the service configuration specified by <typeparamref name="TStartup"/>
        /// </summary>
        /// <param name="serviceCollection">ServiceCollection to apply configuration to</param>
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

        /// <summary>
        /// Empty server required when building a web host.
        /// </summary>
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
