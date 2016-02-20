using FluentAssertions;
using Mendham.Domain.Events;
using Mendham.Domain.DependencyInjection.Ninject.Test.TestObjects;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Ninject.Test
{
    public class RegistrationExtensionsTest : IDisposable
    {
        private IKernel sut;

        public RegistrationExtensionsTest()
        {
            this.sut = new StandardKernel(new DomainEventHandlingModule());
        }

        public void Dispose()
        {
            this.sut.Dispose();
        }

        [Fact]
        public void RegisterDomainEventHandlers_HandlersInAssembly_ReturnsAll()
        {
            var assembly = GetType().GetTypeInfo().Assembly;

            sut.RegisterDomainEventHandlers(assembly);

            var result = sut.GetAll<IDomainEventHandler>();

            result.Should()
                .NotBeEmpty();
            result.Should()
                .ContainItemsAssignableTo<IDomainEventHandler>();
            result.Should()
                .Contain(a => a is Test1DomainEventHandler);
            result.Should()
                .Contain(a => a is Test2DomainEventHandler);
        }

        [Fact]
        public void RegisterDomainFacades_ApplyingToBuilder_ReturnsFacade()
        {
            var assembly = GetType().GetTypeInfo().Assembly;

            sut.RegisterDomainFacades(assembly);

            var facade = sut.Get<TestEntityWithDomainFacade.IFacade>();

            facade.Should().NotBeNull();
        }

        [Fact]
        public void RegisterDomainFacades_DerivedInterface_ReturnsDerivedFacade()
        {
            var assembly = GetType().GetTypeInfo().Assembly;

            sut.RegisterDomainFacades(assembly);

            var facade = sut.Get<DerivedTestEntityWithDomainFacade.IDerivedFacade>();

            facade.Should().NotBeNull();
        }

        [Fact]
        public void RegisterDomainFacades_BaseInterfaceOnAbstractBase_ReturnsNonAbstractFacade()
        {
            var assembly = GetType().GetTypeInfo().Assembly;

            sut.RegisterDomainFacades(assembly);

            var facade = sut.Get<AbstractTestEntityWithDomainFacade.IBaseFacade>();

            facade.
                Should()
                .NotBeNull("there is a non abstract facade assocaited with IBaseFacade")
                .And.BeOfType<DerivedTestEntityWithDomainFacade.DerivedFacade>("this is the one and only non abstract class");
        }
    }
}
