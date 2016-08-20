using Mendham.Domain.Events;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Conventions.Syntax;
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
            assembly.VerifyArgumentNotNull(nameof(assembly));

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
            assembly.VerifyArgumentNotNull(nameof(assembly));

            bindingRoot.RegisterDomainFacades(assembly, Enumerable.Empty<Type>());
        }

        /// <summary>
        /// Registers all concreate classes in the assembly that are assignable from an interface that is assignable from IDomainFacade
        /// </summary>
        /// <param name="interfacesToExclude">Intefaces to not include in domain facade registration (all must derive from IDomainFacade)</param>
        public static void RegisterDomainFacades(this IBindingRoot bindingRoot, Assembly assembly, IEnumerable<Type> interfacesToExclude)
        {
            assembly
                .VerifyArgumentNotNull(nameof(assembly));
            interfacesToExclude
                .VerifyArgumentNotNull(nameof(interfacesToExclude))
                .VerifyArgumentMeetsCriteria(type => !type.Any(IsNotValidDomainFacadeInterface),
                    set => new InvalidDomainFacadeExclusionException(set.First(IsNotValidDomainFacadeInterface)));

            bindingRoot.Bind(a => a
                .From(assembly)
                .SelectAllClasses()
                .InheritedFrom<IDomainFacade>()
                .BindSelection(DomainFacadeSelector(interfacesToExclude))
                .Configure(b => b.InSingletonScope())
            );
        }

        /// <summary>
        /// Builds ServiceSelector delegate with consideration to interfacesToExclude
        /// </summary>
        private static ServiceSelector DomainFacadeSelector(IEnumerable<Type> interfacesToExclude)
        {
            ServiceSelector serviceSelector = (type, baseTypes) =>
                baseTypes
                    .Where(IsDomainFacadeInterface)
                    .Where(a => !interfacesToExclude.Contains(a))
                    .Select(interfaceType => ValidateInterfaceOnlyAssignedOnce(interfaceType, type.GetTypeInfo()));
                    
            return serviceSelector;
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

        private static bool IsNotValidDomainFacadeInterface(Type type)
        {
            var ti = type.GetTypeInfo();

            return !typeof(IDomainFacade).IsAssignableFrom(ti) || !ti.IsInterface;
        }
    }
}
