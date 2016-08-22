using FluentAssertions;
using Mendham.Domain.DependencyInjection.InvalidConcreateBaseEntity;
using Mendham.Domain.DependencyInjection.InvalidMultipleDerivedEntity;
using Mendham.Domain.DependencyInjection.TestObjects;
using Mendham.Events.DependencyInjection.Ninject;
using Ninject;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Ninject.Test
{
    public class RegistrationExtensionsTest : IDisposable
    {
        private IKernel sut;

        public RegistrationExtensionsTest()
        {
            sut = new StandardKernel(new EventHandlingModule());
        }

        public void Dispose()
        {
            sut.Dispose();
        }

        [Fact]
        public void RegisterDomainFacades_ApplyingToBuilder_ReturnsFacade()
        {
            var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;

            sut.RegisterDomainFacades(assembly);

            var facade = sut.Get<TestEntityWithDomainFacade.IFacade>();

            facade.Should().NotBeNull();
        }

        [Fact]
        public void RegisterDomainFacades_DerivedInterface_ReturnsDerivedFacade()
        {
            var assembly = typeof(DerivedTestEntityWithDomainFacade).GetTypeInfo().Assembly;

            sut.RegisterDomainFacades(assembly);

            var facade = sut.Get<DerivedTestEntityWithDomainFacade.IDerivedFacade>();

            facade.Should().NotBeNull();
        }

        [Fact]
        public void RegisterDomainFacades_BaseInterfaceOnAbstractBase_ReturnsNonAbstractFacade()
        {
            var assembly = typeof(AbstractTestEntityWithDomainFacade).GetTypeInfo().Assembly;

            sut.RegisterDomainFacades(assembly);

            var facade = sut.Get<AbstractTestEntityWithDomainFacade.IBaseFacade>();

            facade.
                Should()
                .NotBeNull("there is a non abstract facade assocaited with IBaseFacade")
                .And.BeOfType<DerivedTestEntityWithDomainFacade.DerivedFacade>("this is the one and only non abstract class");
        }

        [Fact]
        public void RegisterDomainFacades_InvalidConditionSharedFacadeBetweenBaseAndDerivedEntity_ThrowsMultipleDomainFacadesFoundException()
        {
            var assembly = typeof(ConcreateBaseEntity).GetTypeInfo().Assembly;

            Action act = () => sut.RegisterDomainFacades(assembly);

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

            Action act = () => sut.RegisterDomainFacades(assembly);

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

            sut.RegisterDomainFacades(assembly, interfacesToIgnore);
            sut.Bind<ConcreateBaseEntity.IBaseFacade>().To<ConcreateBaseEntity.BaseFacade>();

            var result = sut.Get<ConcreateBaseEntity.IBaseFacade>();

            result.Should().NotBeNull();
        }

        [Fact]
        public void RegisterDomainFacades_IgnoreConcreateBaseNotSettingBase_BaseFacadeNotSet()
        {
            var assembly = typeof(ConcreateBaseEntity).GetTypeInfo().Assembly;
            var interfacesToIgnore = typeof(ConcreateBaseEntity.IBaseFacade).AsSingleItemEnumerable();

            sut.RegisterDomainFacades(assembly, interfacesToIgnore);

            var result = sut.TryGet<ConcreateBaseEntity.IBaseFacade>();

            result.Should().BeNull("the base facade is not set");
        }

        [Fact]
        public void RegisterDomainFacades_IgnoreConcreateBase_ReturnsDerivedFacade()
        {
            var assembly = typeof(ConcreateBaseEntity).GetTypeInfo().Assembly;
            var interfacesToIgnore = typeof(ConcreateBaseEntity.IBaseFacade).AsSingleItemEnumerable();

            sut.RegisterDomainFacades(assembly, interfacesToIgnore);

            var result = sut.Get<DerivedFromConcreateBaseEntity.IDerivedFacade>();

            result.Should().NotBeNull();
        }

        [Fact]
        public void RegisterDomainFacades_IgnoreListIncludesNonInterface_ThrowsInvalidDomainFacadeExclusionException()
        {
            var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;
            var nonInterfaceType = typeof(TestEntityWithDomainFacade.Facade);

            Action act = () => sut.RegisterDomainFacades(assembly, nonInterfaceType.AsSingleItemEnumerable());

            act.ShouldThrow<InvalidDomainFacadeExclusionException>()
                .Where(a => a.InvalidType.Equals(nonInterfaceType), "it is not an interface");
        }

        [Fact]
        public void RegisterDomainFacades_IgnoreListIncludesInterfaceNotDerivedFromDomainFacade_ThrowsInvalidDomainFacadeExclusionException()
        {
            var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;
            var interfaceNotDerivedFromDomainFacade = typeof(IUnrelatedInterface);

            Action act = () => sut.RegisterDomainFacades(assembly, interfaceNotDerivedFromDomainFacade.AsSingleItemEnumerable());

            act.ShouldThrow<InvalidDomainFacadeExclusionException>()
                .Where(a => a.InvalidType.Equals(interfaceNotDerivedFromDomainFacade), "the interface is not derived from IDomainFacade");
        }
    }
}
