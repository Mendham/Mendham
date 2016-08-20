using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public class BuilderAssemblyQueryService : IBuilderAssemblyQueryService
    {
        private static ConcurrentDictionary<Assembly, IEnumerable<Assembly>> _assemblies;

        static BuilderAssemblyQueryService()
        {
            _assemblies = new ConcurrentDictionary<Assembly, IEnumerable<Assembly>>();
        }

        public IEnumerable<Assembly> GetAssembliesWithBuilders(Assembly callingAssembly)
        {
            return _assemblies.GetOrAdd(callingAssembly, AddAssembliesWithBuilders);
        }

        private static IEnumerable<Assembly> AddAssembliesWithBuilders(Assembly callingAssembly)
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
                .Distinct()
                .ToList();
        }
    }
}
