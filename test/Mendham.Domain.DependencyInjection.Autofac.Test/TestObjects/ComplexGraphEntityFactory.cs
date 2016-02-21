using Mendham.Domain.DependencyInjection.ComplexDomainGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.Autofac.Test.TestObjects
{
    public class ComplexGraphEntityFactory : IEntityFactory
    {
        private readonly Func<Entity1> entity1Factory;

        public ComplexGraphEntityFactory(Func<Entity1> entity1Factory)
        {
            this.entity1Factory = entity1Factory;
        }

        public Entity1 Create()
        {
            return entity1Factory();
        }
    }
}
