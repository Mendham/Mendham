using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mendham.Testing.AspNetCore
{
    public abstract class ServerFixture : IFixture, IDisposable
    {
        public ServerFixture()
        {
            var builder = GetWebHostBuilder();

            Server = new TestServer(builder);
            Client = Server.CreateClient();
            Services = Server.Host.Services;
        }

        public TestServer Server { get; }
        public HttpClient Client { get; }
        public IServiceProvider Services { get; }

        protected abstract IWebHostBuilder GetWebHostBuilder();
     
        public virtual void ResetFixture()
        {
        }

        public virtual void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }

    public class ServerFixture<TStartup> : ServerFixture where TStartup : class
    {
        protected sealed override IWebHostBuilder GetWebHostBuilder()
        {
            return new WebHostBuilder()
                .ConfigureServices(ServiceConfiguration)
                .UseStartup<TStartup>();
        }

        protected virtual void ServiceConfiguration(IServiceCollection services)
        {
        }
    }
}
