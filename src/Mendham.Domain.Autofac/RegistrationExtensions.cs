using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Mendham.Domain.Events;

namespace Mendham.Domain.Autofac
{
	public static class RegistrationExtensions
	{
		public static void RegisterDomainEventHandlers(this ContainerBuilder builder, Assembly assembly)
		{
			builder
				.RegisterAssemblyTypes(assembly)
				.As<IDomainEventHandler>();
		}

		public static void RegisterDomainFacades(this ContainerBuilder builder, Assembly assembly)
		{
			builder
				.RegisterAssemblyTypes(assembly)
				.Where(a => a
					.GetTypeInfo()
					.IsAssignableFrom(typeof(IDomainFacade).GetTypeInfo()))
				.As(t => t.GetInterfaces()
					.Where(a => a != typeof(IDomainFacade)));
        }

	}
}
