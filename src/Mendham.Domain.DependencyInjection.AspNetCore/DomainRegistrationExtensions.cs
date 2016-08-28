using Mendham;
using Mendham.Domain;
using Mendham.Domain.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DomainRegistrationExtensions
    {
        public static void AddDomain(this IServiceCollection services, params Assembly[] assemblies)
        {
            assemblies.VerifyArgumentNotNullOrEmpty(nameof(assemblies), "One or more assemblies are required");

            services.AddEntities(assemblies);
            services.AddEventHandling()
                .AddEventHandlers(assemblies);

            foreach (var assembly in assemblies)
            {
                services.AddDomainFacadesInternal(assembly, Enumerable.Empty<Type>());
            }
        }

        public static void AddDomainFacades(this IServiceCollection services, params Assembly[] assemblies)
        {
            assemblies.VerifyArgumentNotNullOrEmpty(nameof(assemblies), "One or more assemblies are required");

            services.AddEventHandling();

            foreach (var assembly in assemblies)
            {
                services.AddDomainFacades(assembly, Enumerable.Empty<Type>());
            }   
        }

        public static void AddDomainFacades(this IServiceCollection services, Assembly assembly, IEnumerable<Type> interfacesToExclude)
        {
            services.AddDomainFacadesInternal(assembly, interfacesToExclude);
            services.AddEventHandling();
        }

        private static void AddDomainFacadesInternal(this IServiceCollection services, Assembly assembly, IEnumerable<Type> interfacesToExclude)
        {
            assembly
                .VerifyArgumentNotNull(nameof(assembly));
            interfacesToExclude
                .VerifyArgumentNotNull(nameof(interfacesToExclude))
                .VerifyArgumentMeetsCriteria(type => !type.Any(DomainRegistrationHelpers.IsNotValidDomainFacadeInterface),
                    set => new InvalidDomainFacadeExclusionException(set.First(DomainRegistrationHelpers.IsNotValidDomainFacadeInterface)));

            var concreateTypesInAssembly = DomainRegistrationHelpers.GetTypesAssignableFromIDomainFacade(assembly);

            assembly.GetTypes()
                .Where(DomainRegistrationHelpers.IsAssignableFromIDomainFacade)
                .SelectMany(implementationType => DomainRegistrationHelpers.ValidateAndGetServiceMapping(implementationType, concreateTypesInAssembly, interfacesToExclude),
                    (implementationType, serviceType) => new { implementationType, serviceType })
                .ToList()
                .ForEach(a => services.AddTransient(a.serviceType, a.implementationType));
        }

        public static void AddEntities(this IServiceCollection services, params Assembly[] assemblies)
        {
            assemblies.VerifyArgumentNotNullOrEmpty(nameof(assemblies), "One or more assemblies are required");

            assemblies
                .SelectMany(a => a.GetTypes())
                .Where(a => typeof(IEntity).IsAssignableFrom(a) && !a.GetTypeInfo().IsAbstract)
                .ToList()
                .ForEach(a => services.AddTransient(a));
        }
    }
}
