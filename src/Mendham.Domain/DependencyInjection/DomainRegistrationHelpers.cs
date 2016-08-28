using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Mendham.Domain.DependencyInjection
{
    public static class DomainRegistrationHelpers
    {
        public static IEnumerable<Type> ValidateAndGetServiceMapping(Type typeToBind, IEnumerable<Type> concreateTypesInAssembly, IEnumerable<Type> interfacesToExclude)
        {
            return typeToBind.GetInterfaces()
                .Where(a => !typeof(IDomainFacade).Equals(a) && typeof(IDomainFacade).IsAssignableFrom(a))
                .Where(a => !interfacesToExclude.Contains(a))
                .Select(a => ValidateInterfaceOnlyAssignedOnce(a, concreateTypesInAssembly));
        }

        public static bool IsAssignableFromIDomainFacade(Type type)
        {
            return typeof(IDomainFacade).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract;
        }

        public static IEnumerable<Type> GetTypesAssignableFromIDomainFacade(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(IsAssignableFromIDomainFacade)
                .Select(a => a.GetTypeInfo())
                .Where(a => a.IsClass && !a.IsAbstract)
                .Cast<Type>()
                .ToList();
        }

        private static Type ValidateInterfaceOnlyAssignedOnce(Type interfaceType, IEnumerable<Type> assignableTypes)
        {
            var typesImplementingInterface = assignableTypes
                .Where(a => interfaceType.IsAssignableFrom(a));

            if (typesImplementingInterface.Count() == 1)
            {
                return interfaceType;
            }
            else if (!typesImplementingInterface.Any())
            {
                throw new InvalidOperationException($"No types found to implement interface {interfaceType.FullName}");
            }
            else
            {
                var multipleTypes = new ReadOnlyCollection<Type>(typesImplementingInterface
                    .OrderBy(a => a.FullName)
                    .ToList());

                throw new MultipleDomainFacadesFoundException(interfaceType, multipleTypes);
            }
        }

        public static bool IsNotValidDomainFacadeInterface(Type type)
        {
            return !typeof(IDomainFacade).IsAssignableFrom(type) || !type.GetTypeInfo().IsInterface;
        }
    }
}
