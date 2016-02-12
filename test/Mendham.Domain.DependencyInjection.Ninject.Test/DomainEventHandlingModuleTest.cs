using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Ninject.Modules;
using Ninject;
using Mendham.Domain.Events;
using FluentAssertions;

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
                .And.BeOfType<DomainEventHandlerContainer>();
        }

        [Fact]
        public void DomainEventHandlingModule_RegisterDomainEventLoggerContainer_Resolves()
        {
            var result = sut.Get<IDomainEventLoggerContainer>();

            result.Should()
                .NotBeNull()
                .And.BeOfType<DomainEventLoggerContainer>();
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
        public void DomainEventHandlingModule_RegisterDomainEventLoggerContainer_IsSameInstance()
        {
            var expectedContainer = sut.Get<IDomainEventLoggerContainer>();
            var result = sut.Get<IDomainEventLoggerContainer>();

            result.Should()
                .BeSameAs(expectedContainer);
        }
    }
}
