using Autofac;
using FluentAssertions;
using Mendham.Domain.DependencyInjection.InvalidConcreateBaseEntity;
using Mendham.Domain.DependencyInjection.InvalidMultipleDerivedEntity;
using Mendham.Domain.DependencyInjection.TestObjects;
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
			var assembly = typeof(Test1DomainEventHandler).GetTypeInfo().Assembly;

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
			var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;

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
            var assembly = typeof(DerivedTestEntityWithDomainFacade).GetTypeInfo().Assembly;

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
            var assembly = typeof(AbstractTestEntityWithDomainFacade).GetTypeInfo().Assembly;

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
            var assembly = typeof(IUnrelatedInterface).GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterDomainFacades(assembly);

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var result = sut.IsRegistered<IUnrelatedInterface>();

                result.Should().BeFalse();
            }
        }

        [Fact]
        public void RegisterDomainFacades_InvalidConditionSharedFacadeBetweenBaseAndDerivedEntity_ThrowsMultipleDomainFacadesFoundException()
        {
            var assembly = typeof(ConcreateBaseEntity).GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterDomainFacades(assembly);

            Action act = () => builder.Build();

            act.ShouldThrow<MultipleDomainFacadesFoundException>()
                .Where(a => a.InterfaceToBind.Equals(typeof(ConcreateBaseEntity.IBaseFacade)), "IBaseFacade is implemented in two concreate classes")
                .Where(a => a.TypesImplementingInterface.Count() == 2, "There are two classes that implement the base facade")
                .Where(a => a.TypesImplementingInterface.Contains(typeof(ConcreateBaseEntity.BaseFacade)), "BaseFacade is concreate and implements IBaseFacade")
                .Where(a => a.TypesImplementingInterface.Contains(typeof(DerivedFromConcreateBaseEntity.DerivedFacade)), "DerivedFacade is concreate and implements IBaseFacade");
        }

        [Fact]
        public void RegisterDomainFacades_InvalidConditionSharedFacadeMultipleDerivedEntity_ThrowsMultipleDomainFacadesFoundException()
        {
            var assembly = typeof(AbstractBaseEntity).GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterDomainFacades(assembly);

            Action act = () => builder.Build();

            act.ShouldThrow<MultipleDomainFacadesFoundException>()
                .Where(a => a.InterfaceToBind.Equals(typeof(AbstractBaseEntity.IBaseFacade)), "IBaseFacade is implemented in two concreate classes")
                .Where(a => a.TypesImplementingInterface.Count() == 2, "There are two classes that implement the base facade")
                .Where(a => a.TypesImplementingInterface.Contains(typeof(DerivedFromAbstractBaseEntity.DerivedFacade)), "DerivedFacade in DerivedFromAbstractBaseEntity implements IBaseFacade")
                .Where(a => a.TypesImplementingInterface.Contains(typeof(AltDerivedFromAbstractBaseEntity.DerivedFacade)), "DerivedFacade in DerivedFromAbstractBaseEntity implements IBaseFacade");
        }

        [Fact]
		public void RegisterDomainFacades_ApplyingToBuilder_ReturnsEntity()
		{
			var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;

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