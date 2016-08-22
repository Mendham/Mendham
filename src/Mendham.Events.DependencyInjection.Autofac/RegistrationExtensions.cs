using Autofac;
using System.Reflection;

namespace Mendham.Events.DependencyInjection.Autofac
{
    public static class RegistrationExtensions
    {
        /// <summary>
        /// Registers all event handlers found in the assembly as a singleton
        /// </summary>
		public static void RegisterEventHandlers(this ContainerBuilder builder, Assembly assembly)
        {
            assembly.VerifyArgumentNotNull(nameof(assembly));

            builder
                .RegisterAssemblyTypes(assembly)
                .As<IEventHandler>()
                .SingleInstance();
        }
    }
}
