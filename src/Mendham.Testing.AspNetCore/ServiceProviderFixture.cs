using Microsoft.Extensions.DependencyInjection;
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
}
