using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public class BuilderQueryService : IBuilderQueryService
    {
        private readonly IBuilderAssemblyQueryService builderAssemblyQuerySvc;

        public BuilderQueryService()
            : this(new BuilderAssemblyQueryService())
        { }

        public BuilderQueryService(IBuilderAssemblyQueryService builderAssemblyQuerySvc)
        {
            builderAssemblyQuerySvc.VerifyArgumentNotDefaultValue("IBuilderAssemblyQueryService is required");

            this.builderAssemblyQuerySvc = builderAssemblyQuerySvc;
        }

        public IEnumerable<Type> GetBuilderTypes(Assembly callingAssembly)
        {
            return builderAssemblyQuerySvc
                .GetAssembliesWithBuilders(callingAssembly)
                .AsParallel()
                .SelectMany(GetBuilderTypesInAssembly);
        }

        private static IEnumerable<Type> GetBuilderTypesInAssembly(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(BuilderExtensions.ImplementsIBuilder);
        }
    }
}
