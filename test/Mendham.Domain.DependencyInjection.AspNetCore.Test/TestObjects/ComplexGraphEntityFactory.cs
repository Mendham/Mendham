using Mendham.Domain.DependencyInjection.ComplexDomainGraph;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mendham.Domain.DependencyInjection.AspNetCore.Test.TestObjects
{
    public class ComplexGraphEntityFactory : IEntityFactory
    {
        private IServiceProvider _serviceProvider;

        public ComplexGraphEntityFactory(IServiceProvider resolutionRoot)
        {
            _serviceProvider = resolutionRoot;
        }

        public Entity1 Create()
        {
            return _serviceProvider.GetService<Entity1>();
        }
    }
}
