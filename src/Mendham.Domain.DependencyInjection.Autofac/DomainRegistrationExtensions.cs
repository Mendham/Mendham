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
                .VerifyArgumentMeetsCriteria(type => !type.Any(DomainRegistrationHelpers.IsNotValidDomainFacadeInterface),
                    set => new InvalidDomainFacadeExclusionException(set.First(DomainRegistrationHelpers.IsNotValidDomainFacadeInterface)));

            var concreateTypesInAssembly = DomainRegistrationHelpers.GetTypesAssignableFromIDomainFacade(assembly);

            builder
                .RegisterAssemblyTypes(assembly)
                .Where(DomainRegistrationHelpers.IsAssignableFromIDomainFacade)
                .As(t => DomainRegistrationHelpers.ValidateAndGetServiceMapping(t, concreateTypesInAssembly, interfacesToExclude))
                .InstancePerDependency();
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
    }
}