using Ninject.Extensions.Conventions;
using Ninject.Syntax;
using System.Linq;
using System.Reflection;

namespace Mendham.Events.DependencyInjection.Ninject
{
    public static class RegistrationExtensions
    {
        /// <summary>
        /// Registers all event handlers found in the assembly as a singleton
        /// </summary>
        public static void RegisterEventHandlers(this IBindingRoot bindingRoot, Assembly assembly)
        {
            assembly.VerifyArgumentNotNull(nameof(assembly));

            bindingRoot.Bind(a => a
                .From(assembly)
                .SelectAllClasses()
                .InheritedFrom(typeof(IEventHandler<>))
                .BindSelection((t, types) => types.Where(b => b.Equals(typeof(IEventHandler))))
                .Configure(b => b.InSingletonScope())
            );
        }
    }
}
