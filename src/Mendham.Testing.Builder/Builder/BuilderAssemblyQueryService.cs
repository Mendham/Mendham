using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public class BuilderAssemblyQueryService : IBuilderAssemblyQueryService
    {
        public IEnumerable<Assembly> GetAssembliesWithBuilders(Assembly callingAssembly)
        {
            var referencedAssemblyNames = callingAssembly
                .GetReferencedAssemblies();

            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Join(referencedAssemblyNames,
                    assembly => assembly.GetName(),
                    assemblyName => assemblyName,
                    (assembly, name) => assembly)
                .Union(callingAssembly.AsSingleItemEnumerable())
                .Distinct();
        }
    }
}
