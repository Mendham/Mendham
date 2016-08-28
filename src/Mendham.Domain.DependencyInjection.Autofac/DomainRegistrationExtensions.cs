using Autofac;
using Mendham.Domain;
using Mendham.Domain.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Mendham.DependencyInjection.Autofac
{
    public static class DomainRegistrationExtensions
	{
        /// <summary>
        /// Registers all concreate classes in the assembly that are assignable from an interface that is assignable from IDomainFacade
        /// </summary>
		public static void RegisterDomainFacades(this ContainerBuilder builder, Assembly assembly)
		{
            assembly.VerifyArgumentNotNull(nameof(assembly));

            builder.RegisterDomainFacades(assembly, Enumerable.Empty<Type>());
        }

        /// <summary>
        /// Registers all concreate classes in the assembly that are assignable from an interface that is assignable from IDomainFacade
        /// </summary>
        /// <param name="interfacesToExclude">Intefaces to not include in domain facade registration (all must derive from IDomainFacade)</param>
		public static void RegisterDomainFacades(this ContainerBuilder builder, Assembly assembly, IEnumerable<Type> interfacesToExclude)
        {
            assembly
                .VerifyArgumentNotNull(nameof(assembly));
            interfacesToExclude
                .VerifyArgumentNotNull(nameof(interfacesToExclude))
                .VerifyArgumentMeetsCriteria(type => !type.Any(IsNotValidDomainFacadeInterface),
                    set => new InvalidDomainFacadeExclusionException(set.First(IsNotValidDomainFacadeInterface)));

            var concreateTypesInAssembly = GetTypesAssignableFromIDomainFacade(assembly);

            builder
                .RegisterAssemblyTypes(assembly)
                .Where(IsAssignableFromIDomainFacade)
                .As(t => ValidateAndGetServiceMapping(t, concreateTypesInAssembly, interfacesToExclude))
                .SingleInstance();
        }

        /// <summary>
        /// Register all concreate types that implement IEntity with one instance per dependency
        /// </summary>
        public static void RegisterEntities(this ContainerBuilder builder, Assembly assembly)
        {
            assembly.VerifyArgumentNotNull(nameof(assembly));

            builder
                .RegisterAssemblyTypes(assembly)
                .Where(a => typeof(IEntity).IsAssignableFrom(a))
                .InstancePerDependency();
        }

        private static IEnumerable<Type> ValidateAndGetServiceMapping(Type typeToBind, IEnumerable<Type> concreateTypesInAssembly, IEnumerable<Type> interfacesToExclude)
        {
            return typeToBind.GetInterfaces()
                .Where(a => !typeof(IDomainFacade).Equals(a) && typeof(IDomainFacade).IsAssignableFrom(a))
                .Where(a => !interfacesToExclude.Contains(a))
                .Select(a => ValidateInterfaceOnlyAssignedOnce(a, concreateTypesInAssembly));
        }

        private static bool IsAssignableFromIDomainFacade(Type type)
        {
            return typeof(IDomainFacade).IsAssignableFrom(type);
        }

        private static IEnumerable<Type> GetTypesAssignableFromIDomainFacade(Assembly assembly)
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

        private static bool IsNotValidDomainFacadeInterface(Type type)
        {
            return !typeof(IDomainFacade).IsAssignableFrom(type) || !type.GetTypeInfo().IsInterface;
        }
    }
}