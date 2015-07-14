using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using Mendham.Domain.Autofac.Test.Data;
using Mendham.Domain.Events;
using Xunit;

namespace Mendham.Domain.Autofac.Test
{
	public class RegistrationExtensionsTest
	{
		[Fact]
		public void RegisterDomainEventHandlers_HandlersInAssembly_ReturnsAll()
		{
			var assembly = typeof(RegistrationExtensionsTest).GetTypeInfo().Assembly;

			var builder = new ContainerBuilder();
			builder.RegisterDomainEventHandlers(assembly);

			using (var sut = builder.Build().BeginLifetimeScope())
			{
				var result = sut.Resolve<IEnumerable<IDomainEventHandler>>();

				result.Should()
					.NotBeEmpty();
				result.Should()
					.ContainItemsAssignableTo<IDomainEventHandler>();
				result.Should()
					.Contain(a => a is Test1DomainEventHandler);
				result.Should()
					.Contain(a => a is Test2DomainEventHandler);
			}
		}
    }
}
