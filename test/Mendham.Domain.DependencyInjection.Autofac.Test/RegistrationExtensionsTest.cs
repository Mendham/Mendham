using Autofac;
using FluentAssertions;
using Mendham.DependencyInjection.Autofac;
using Mendham.Domain.DependencyInjection.InvalidConcreateBaseEntity;
using Mendham.Domain.DependencyInjection.InvalidMultipleDerivedEntity;
using Mendham.Domain.DependencyInjection.TestObjects;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Autofac.Test
{
    public class RegistrationExtensionsTest
	{
		[Fact]
		public void RegisterDomainFacades_ApplyingToBuilder_ReturnsFacade()
		{
			var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;

			var builder = new ContainerBuilder();
			builder.RegisterModule<EventHandlingModule>();
			builder.RegisterDomainFacades(assembly);

			using (var sut = builder.Build().BeginLifetimeScope())
			{
				var result = sut.IsRegistered<TestEntityWithDomainFacade.IFacade>();

				result.Should().BeTrue();
			}
        }

        [Fact]
        public void RegisterDomainFacades_DerivedInterface_ReturnsDerivedFacade()
        {
            var assembly = typeof(DerivedTestEntityWithDomainFacade).GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();
            builder.RegisterDomainFacades(assembly);

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var result = sut.IsRegistered<DerivedTestEntityWithDomainFacade.IDerivedFacade>();

				result.Should().BeTrue();
            }
        }

        [Fact]
        public void RegisterDomainFacades_BaseInterfaceOnAbstractBase_ReturnsNonAbstractFacade()
        {
            var assembly = typeof(AbstractTestEntityWithDomainFacade).GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();
            builder.RegisterDomainFacades(assembly);

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var facade = sut.Resolve<AbstractTestEntityWithDomainFacade.IBaseFacade>();

                facade.Should()
                    .NotBeNull("there is a non abstract facade assocaited with IBaseFacade")
                    .And.BeOfType<DerivedTestEntityWithDomainFacade.DerivedFacade>("this is the one and only non abstract class");
            }
        }

        [Fact]
        public void RegisterDomainFacades_UnrelatedInterface_NotBound()
        {
            var assembly = typeof(IUnrelatedInterface).GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();
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
            builder.RegisterModule<EventHandlingModule>();
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
            builder.RegisterModule<EventHandlingModule>();
            builder.RegisterDomainFacades(assembly);

            Action act = () => builder.Build();

            act.ShouldThrow<MultipleDomainFacadesFoundException>()
                .Where(a => a.InterfaceToBind.Equals(typeof(AbstractBaseEntity.IBaseFacade)), "IBaseFacade is implemented in two concreate classes")
                .Where(a => a.TypesImplementingInterface.Count() == 2, "There are two classes that implement the base facade")
                .Where(a => a.TypesImplementingInterface.Contains(typeof(DerivedFromAbstractBaseEntity.DerivedFacade)), "DerivedFacade in DerivedFromAbstractBaseEntity implements IBaseFacade")
                .Where(a => a.TypesImplementingInterface.Contains(typeof(AltDerivedFromAbstractBaseEntity.DerivedFacade)), "DerivedFacade in DerivedFromAbstractBaseEntity implements IBaseFacade");
        }

        [Fact]
        public void RegisterDomainFacades_IgnoreConcreateBaseSetManually_ReturnsBaseFacade()
        {
            var assembly = typeof(ConcreateBaseEntity).GetTypeInfo().Assembly;
            var interfacesToIgnore = typeof(ConcreateBaseEntity.IBaseFacade).AsSingleItemEnumerable();

            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();
            builder.RegisterDomainFacades(assembly, interfacesToIgnore);
            builder.RegisterType<ConcreateBaseEntity.BaseFacade>().As<ConcreateBaseEntity.IBaseFacade>();

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var result = sut.Resolve<ConcreateBaseEntity.IBaseFacade>();

                result.Should().NotBeNull();
            }
        }

        [Fact]
        public void RegisterDomainFacades_IgnoreConcreateBaseNotSettingBase_BaseFacadeNotSet()
        {
            var assembly = typeof(ConcreateBaseEntity).GetTypeInfo().Assembly;
            var interfacesToIgnore = typeof(ConcreateBaseEntity.IBaseFacade).AsSingleItemEnumerable();

            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();
            builder.RegisterDomainFacades(assembly, interfacesToIgnore);

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var result = sut.IsRegistered<ConcreateBaseEntity.IBaseFacade>();

                result.Should().BeFalse("the base facade is not set");
            }
        }

        [Fact]
        public void RegisterDomainFacades_IgnoreConcreateBase_ReturnsDerivedFacade()
        {
            var assembly = typeof(ConcreateBaseEntity).GetTypeInfo().Assembly;
            var interfacesToIgnore = typeof(ConcreateBaseEntity.IBaseFacade).AsSingleItemEnumerable();

            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();
            builder.RegisterDomainFacades(assembly, interfacesToIgnore);

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var result = sut.Resolve<DerivedFromConcreateBaseEntity.IDerivedFacade>();

                result.Should().NotBeNull();
            }
        }

        [Fact]
        public void RegisterDomainFacades_IgnoreListIncludesNonInterface_ThrowsInvalidDomainFacadeExclusionException()
        {
            var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;
            var nonInterfaceType = typeof(TestEntityWithDomainFacade.Facade);
            var builder = new ContainerBuilder();

            Action act = () => builder.RegisterDomainFacades(assembly, nonInterfaceType.AsSingleItemEnumerable());

            act.ShouldThrow<InvalidDomainFacadeExclusionException>()
                .Where(a => a.InvalidType.Equals(nonInterfaceType), "it is not an interface");
        }

        [Fact]
        public void RegisterDomainFacades_IgnoreListIncludesInterfaceNotDerivedFromDomainFacade_ThrowsInvalidDomainFacadeExclusionException()
        {
            var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;
            var interfaceNotDerivedFromDomainFacade = typeof(IUnrelatedInterface);
            var builder = new ContainerBuilder();

            Action act = () => builder.RegisterDomainFacades(assembly, interfaceNotDerivedFromDomainFacade.AsSingleItemEnumerable());

            act.ShouldThrow<InvalidDomainFacadeExclusionException>()
                .Where(a => a.InvalidType.Equals(interfaceNotDerivedFromDomainFacade), "the interface is not derived from IDomainFacade");
        }

        [Fact]
		public void RegisterDomainFacades_ApplyingToBuilder_ReturnsEntity()
		{
			var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;

			var builder = new ContainerBuilder();
			builder.RegisterModule<EventHandlingModule>();
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