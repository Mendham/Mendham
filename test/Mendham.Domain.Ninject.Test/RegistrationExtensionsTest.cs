using FluentAssertions;
using Mendham.Domain.Events;
using Mendham.Domain.Ninject.Test.TestObjects;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Ninject.Test
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
            var assembly = typeof(RegistrationExtensionsTest).GetTypeInfo().Assembly;

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
            var assembly = typeof(RegistrationExtensionsTest).GetTypeInfo().Assembly;

            sut.RegisterDomainFacades(assembly);

            var facade = sut.Get<TestEntityWithDomainFacade.IFacade>();

            facade.Should().NotBeNull();
        }
    }
}
