using Autofac;
using FluentAssertions;
using Mendham.Domain.DependencyInjection.Autofac.Test.Data;
using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Autofac.Test
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

		[Fact]
		public void RegisterDomainFacades_ApplyingToBuilder_ReturnsFacade()
		{
			var assembly = typeof(RegistrationExtensionsTest).GetTypeInfo().Assembly;

			var builder = new ContainerBuilder();
			builder.RegisterModule<DomainEventHandlingModule>();
			builder.RegisterDomainFacades(assembly);

			using (var sut = builder.Build().BeginLifetimeScope())
			{
				var facade = sut.Resolve<TestEntityWithDomainFacade.IFacade>();

				facade.Should().NotBeNull();
			}
		}

		[Fact]
		public void RegisterDomainFacades_ApplyingToBuilder_ReturnsEntity()
		{
			var assembly = typeof(RegistrationExtensionsTest).GetTypeInfo().Assembly;

			var builder = new ContainerBuilder();
			builder.RegisterModule<DomainEventHandlingModule>();
			builder.RegisterDomainFacades(assembly);

			using (var sut = builder.Build().BeginLifetimeScope())
			{
				var factory = sut.Resolve<TestEntityWithDomainFacade.Factory>();

				var entity = factory(7);

				entity.Should().NotBeNull();
				entity.HasValidFacade().Should().BeTrue();
			}
		}
	}
}