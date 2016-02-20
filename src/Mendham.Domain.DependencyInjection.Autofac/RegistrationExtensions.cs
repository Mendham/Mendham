using Autofac;
using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Mendham.Domain.DependencyInjection.Autofac
{
    public static class RegistrationExtensions
	{
        /// <summary>
        /// Registers all domain event handlers found in the assembly
        /// </summary>
		public static void RegisterDomainEventHandlers(this ContainerBuilder builder, Assembly assembly)
		{
			builder
				.RegisterAssemblyTypes(assembly)
				.As<IDomainEventHandler>()
				.InstancePerLifetimeScope();
		}

        /// <summary>
        /// Registers all concreate classes in the assembly that are assignable from an interface that is assignable from IDomainFacade
        /// </summary>
		public static void RegisterDomainFacades(this ContainerBuilder builder, Assembly assembly)
		{
			builder
				.RegisterAssemblyTypes(assembly)
				.Where(a => typeof(Entity)
					.GetTypeInfo()
					.IsAssignableFrom(a.GetTypeInfo()))
				.InstancePerDependency();

            var concreateTypesInAssembly = GetTypesAssignableFromIDomainFacade(assembly);

			builder
				.RegisterAssemblyTypes(assembly)
				.Where(IsAssignableFromIDomainFacade)
				.As(t => ValidateAndGetServiceMapping(t, concreateTypesInAssembly))
				.InstancePerLifetimeScope();
        }

        private static IEnumerable<Type> ValidateAndGetServiceMapping(Type typeToBind, IEnumerable<Type> concreateTypesInAssembly)
        {
            return typeToBind.GetInterfaces()
                .Where(a => !typeof(IDomainFacade).Equals(a) && typeof(IDomainFacade).IsAssignableFrom(a))
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
	}
}