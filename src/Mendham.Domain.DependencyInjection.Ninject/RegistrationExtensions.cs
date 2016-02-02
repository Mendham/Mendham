using Mendham.Domain.Events;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.Ninject
{
    public static class RegistrationExtensions
    {
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

        private static IEnumerable<Type> DomainFacadeSelector(Type type, IEnumerable<Type> types)
        {
            return types
                .Where(IsDomainFacadeInterface);
        }

        private readonly static TypeInfo domainFacadeInterface = typeof(IDomainFacade).GetTypeInfo();

        private static bool IsDomainFacadeInterface(Type type)
        {
            return type.IsInterface &&
                !type.Equals(typeof(IDomainFacade)) &&
                domainFacadeInterface.IsAssignableFrom(type.GetTypeInfo());
        }
    }
}
