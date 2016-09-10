using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.Features;

namespace Mendham.Testing.AspNetCore
{
    public abstract class WebHostFixture : IWebHostFixture
    {
        private IWebHost _webHost;

        public WebHostFixture()
        {
            _webHost = GetWebHostBuilder().Build();
            Services = _webHost.Services;
            ServerFeatures = _webHost.ServerFeatures;
        }

        public IServiceProvider Services { get; }
        public IFeatureCollection ServerFeatures { get; }

        protected abstract IWebHostBuilder GetWebHostBuilder();

        protected abstract void ServiceConfiguration(IServiceCollection serviceCollection);

        public virtual void ResetFixture()
        {
        }

        public virtual void Dispose()
        {
            _webHost?.Dispose();
        }
    }

    public abstract class WebHostFixture<TStartup> : WebHostFixture, IDisposable where TStartup : class
    {
        protected override sealed IWebHostBuilder GetWebHostBuilder()
        {
            return new WebHostBuilder()
                .ConfigureServices(ServiceConfiguration)
                .UseStartup<TStartup>();
        }
    }
}
