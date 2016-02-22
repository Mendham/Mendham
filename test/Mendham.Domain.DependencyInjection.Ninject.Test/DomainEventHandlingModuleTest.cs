using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Ninject.Modules;
using Ninject;
using Mendham.Domain.Events;
using FluentAssertions;
using Mendham.Domain.Events.Components;

namespace Mendham.Domain.DependencyInjection.Ninject.Test
{
    public class DomainEventHandlingModuleTest : IDisposable
    {
        private IKernel sut;

        public DomainEventHandlingModuleTest()
        {
            this.sut = new StandardKernel(new DomainEventHandlingModule());
        }

        public void Dispose()
        {
            this.sut.Dispose();
        }

        [Fact]
        public void DomainEventHandlingModule_RegisterDomainEventPublisher_Resolves()
        {
            var result = sut.Get<IDomainEventPublisher>();

            result.Should()
                .NotBeNull()
                .And.BeOfType<DomainEventPublisher>();
        }

        [Fact]
        public void DomainEventHandlingModule_RegisterDomainEventHandlerContainer_Resolves()
        {
            var result = sut.Get<IDomainEventHandlerContainer>();

            result.Should()
                .NotBeNull()
                .And.BeOfType<DefaultDomainEventHandlerContainer>();
        }

        [Fact]
        public void DomainEventHandlingModule_RegisterDomainEventHandlerProcessor_Resolves()
        {
            var result = sut.Get<IDomainEventHandlerProcessor>();

            result.Should()
                .NotBeNull()
                .And.BeOfType<DomainEventHandlerProcessor>();
        }

        [Fact]
        public void DomainEventHandlingModule_RegisterDomainEventPublisher_IsSameInstance()
        {
            var expectedPublisher = sut.Get<IDomainEventPublisher>();
            var result = sut.Get<IDomainEventPublisher>();

            result.Should()
                .BeSameAs(expectedPublisher);
        }

        [Fact]
        public void DomainEventHandlingModule_RegisterDomainEventHandlerContainer_IsSameInstance()
        {
            var expectedContainer = sut.Get<IDomainEventHandlerContainer>();
            var result = sut.Get<IDomainEventHandlerContainer>();

            result.Should()
                .BeSameAs(expectedContainer);
        }

        [Fact]
        public void DomainEventHandlingModule_RegisterDomainEventHandlerProcessor_IsSameInstance()
        {
            var expectedProcessor = sut.Get<IDomainEventHandlerProcessor>();
            var result = sut.Get<IDomainEventHandlerProcessor>();

            result.Should()
                .BeSameAs(expectedProcessor);
        }
    }
}
