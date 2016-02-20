using Mendham.Domain.Events;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.Ninject
{
    public static class RegistrationExtensions
    {
        /// <summary>
        /// Registers all domain event handlers found in the assembly
        /// </summary>
        public static void RegisterDomainEventHandlers(this IBindingRoot bindingRoot, Assembly assembly)
        {
            bindingRoot.Bind(a => a
                .From(assembly)
                .SelectAllClasses()
                .InheritedFrom(typeof(IDomainEventHandler<>))
                .BindSelection((t, types) => types.Where(b => b.Equals(typeof(IDomainEventHandler))))
                .Configure(b => b.InSingletonScope())
            );
        }

        /// <summary>
        /// Registers all concreate classes in the assembly that are assignable from an interface that is assignable from IDomainFacade
        /// </summary>
        public static void RegisterDomainFacades(this IBindingRoot bindingRoot, Assembly assembly)
        {
            bindingRoot.Bind(a => a
                .From(assembly)
                .SelectAllClasses()
                .InheritedFrom<IDomainFacade>()
                .BindSelection(DomainFacadeSelector)
                .Configure(b => b.InSingletonScope())
            );
        }

        private static IEnumerable<Type> DomainFacadeSelector(Type typeToBind, IEnumerable<Type> assignableTypes)
        {
            return assignableTypes
                .Where(IsDomainFacadeInterface)
                .Select(interfaceType => ValidateInterfaceOnlyAssignedOnce(interfaceType, typeToBind.GetTypeInfo()));
        }

        private readonly static TypeInfo domainFacadeInterface = typeof(IDomainFacade).GetTypeInfo();

        private static bool IsDomainFacadeInterface(Type type)
        {
            return type.IsInterface &&
                !type.Equals(typeof(IDomainFacade)) &&
                domainFacadeInterface.IsAssignableFrom(type.GetTypeInfo());
        }


        /// <summary>
        /// Only one non abstract class can be assigned per interface. This method validates that condition
        /// and throws an exception if there is an issue
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="typeToBind"></param>
        /// <returns></returns>
        private static Type ValidateInterfaceOnlyAssignedOnce(Type interfaceType, TypeInfo typeToBind)
        {
            var assembly = typeToBind.Assembly;

            var additionalTypes = assembly.GetTypes()
                .Where(a => !typeToBind.Equals(a))
                .Where(a => a.IsClass && !a.IsAbstract)
                .Where(a => interfaceType.IsAssignableFrom(a));

            if (additionalTypes.Any())
            {
                var allConcreateTypes = additionalTypes
                    .ToList();

                allConcreateTypes.Insert(0, typeToBind);

                allConcreateTypes = allConcreateTypes
                    .OrderBy(a => a.FullName)
                    .ToList();

                throw new MultipleDomainFacadesFoundException(interfaceType, 
                    new ReadOnlyCollection<Type>(allConcreateTypes));
            }

            return interfaceType;
        }
    }
}
