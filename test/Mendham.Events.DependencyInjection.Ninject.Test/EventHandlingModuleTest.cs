using FluentAssertions;
using Mendham.DependencyInjection.Ninject;
using Mendham.Events.Components;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Events.DependencyInjection.Ninject.Test
{
    public class EventHandlingModuleTest : IDisposable
    {
        private IKernel sut;

        public EventHandlingModuleTest()
        {
            sut = new StandardKernel(new EventHandlingModule());
        }

        public void Dispose()
        {
            sut.Dispose();
        }

        [Fact]
        public void EventHandlingModule_RegisterDomainEventPublisher_Resolves()
        {
            var result = sut.Get<IEventPublisher>();

            result.Should()
                .NotBeNull()
                .And.BeOfType<EventPublisher>();
        }

        [Fact]
        public void EventHandlingModule_RegisterEventHandlerContainer_Resolves()
        {
            var result = sut.Get<IEventHandlerContainer>();

            result.Should()
                .NotBeNull()
                .And.BeOfType<DefaultEventHandlerContainer>();
        }

        [Fact]
        public void EventHandlingModule_RegisterEventHandlerProcessor_Resolves()
        {
            var result = sut.Get<IEventHandlerProcessor>();

            result.Should()
                .NotBeNull()
                .And.BeOfType<EventHandlerProcessor>();
        }

        [Fact]
        public void EventHandlingModule_RegisterEventLoggerProcessor_Resolves()
        {
            var result = sut.Get<IEventLoggerProcessor>();

            result.Should()
                .NotBeNull()
                .And.BeOfType<EventLoggerProcessor>();
        }

        [Fact]
        public void EventHandlingModule_RegisterDomainEventPublisher_IsSameInstance()
        {
            var expectedPublisher = sut.Get<IEventPublisher>();
            var result = sut.Get<IEventPublisher>();

            result.Should()
                .BeSameAs(expectedPublisher);
        }

        [Fact]
        public void EventHandlingModule_RegisterEventHandlerContainer_IsSameInstance()
        {
            var expectedContainer = sut.Get<IEventHandlerContainer>();
            var result = sut.Get<IEventHandlerContainer>();

            result.Should()
                .BeSameAs(expectedContainer);
        }

        [Fact]
        public void EventHandlingModule_RegisterEventHandlerProcessor_IsSameInstance()
        {
            var expectedHandlerProcessor = sut.Get<IEventHandlerProcessor>();
            var result = sut.Get<IEventHandlerProcessor>();

            result.Should()
                .BeSameAs(expectedHandlerProcessor);
        }

        [Fact]
        public void EventHandlingModule_RegisterEventLoggerProcessor_IsSameInstance()
        {
            var expectedLoggerProcessor = sut.Get<IEventLoggerProcessor>();
            var result = sut.Get<IEventLoggerProcessor>();

            result.Should()
                .BeSameAs(expectedLoggerProcessor);
        }
    }
}
