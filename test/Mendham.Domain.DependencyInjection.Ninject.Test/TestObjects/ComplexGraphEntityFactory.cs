using Mendham.Domain.DependencyInjection.ComplexDomainGraph;
using Ninject;
using Ninject.Syntax;

namespace Mendham.Domain.DependencyInjection.Ninject.Test.TestObjects
{
    public class ComplexGraphEntityFactory : IEntityFactory
    {
        private IResolutionRoot resolutionRoot;

        public ComplexGraphEntityFactory(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }

        public Entity1 Create()
        {
            return resolutionRoot.Get<Entity1>();
        }
    }
}
