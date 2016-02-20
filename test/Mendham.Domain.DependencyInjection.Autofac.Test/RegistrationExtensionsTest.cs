﻿using Autofac;
using FluentAssertions;
using Mendham.Domain.DependencyInjection.Autofac.Test.TestObjects;
using Mendham.Domain.DependencyInjection.InvalidTestEntity;
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
			var assembly = GetType().GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
			builder.RegisterDomainEventHandlers(assembly);

            // This is needed one of the test handlersin the assembly (not used here) injects a publisher
            builder.RegisterModule<DomainEventHandlingModule>();

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
			var assembly = GetType().GetTypeInfo().Assembly;

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
        public void RegisterDomainFacades_DerivedInterface_ReturnsDerivedFacade()
        {
            var assembly = GetType().GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterDomainFacades(assembly);

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var facade = sut.Resolve<DerivedTestEntityWithDomainFacade.IDerivedFacade>();

                facade.Should().NotBeNull();
            }
        }

        [Fact]
        public void RegisterDomainFacades_BaseInterfaceOnAbstractBase_ReturnsNonAbstractFacade()
        {
            var assembly = GetType().GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterDomainFacades(assembly);

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var facade = sut.Resolve<AbstractTestEntityWithDomainFacade.IBaseFacade>();

                facade.
                Should()
                .NotBeNull("there is a non abstract facade assocaited with IBaseFacade")
                .And.BeOfType<DerivedTestEntityWithDomainFacade.DerivedFacade>("this is the one and only non abstract class");
            }
        }

        [Fact]
        public void RegisterDomainFacades_UnrelatedInterface_NotBound()
        {
            var assembly = GetType().GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterDomainFacades(assembly);

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var facade = sut.ResolveOptional<IUnrelatedInterface>();

                facade.Should().BeNull();
            }
        }

        [Fact]
        public void RegisterDomainFacades_InvalidEntityCondition_ThrowsMultipleDomainFacadesFoundException()
        {
            var assembly = typeof(BaseEntity).GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterDomainFacades(assembly);

            Action act = () => builder.Build();

            act.ShouldThrow<MultipleDomainFacadesFoundException>()
                .Where(a => a.InterfaceToBind.Equals(typeof(BaseEntity.IBaseFacade)), "IBaseFacade is implemented in two concreate classes")
                .Where(a => a.TypesImplementingInterface.Count() == 2, "There are two classes that implement the base facade")
                .Where(a => a.TypesImplementingInterface.Contains(typeof(BaseEntity.BaseFacade)), "BaseFacade is concreate and implements IBaseFacade")
                .Where(a => a.TypesImplementingInterface.Contains(typeof(DerivedEntity.DerivedFacade)), "DerivedFacade is concreate and implements IBaseFacade");
        }

        [Fact]
		public void RegisterDomainFacades_ApplyingToBuilder_ReturnsEntity()
		{
			var assembly = GetType().GetTypeInfo().Assembly;

			var builder = new ContainerBuilder();
			builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterEntities(assembly);
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