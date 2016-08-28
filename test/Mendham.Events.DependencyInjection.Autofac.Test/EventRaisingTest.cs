using Autofac;
using FluentAssertions;
using Mendham.DependencyInjection.Autofac;
using Mendham.Events.DependencyInjection.TestObjects;
using Mendham.Events.DependencyInjection.TrackableTestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Events.DependencyInjection.Autofac.Test
{
    public class EventRaisingTest
    {
        [Fact]
        public async Task Raise_SingleEvent_IsLogged()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<EventHandlingModule>();

            builder.RegisterType<TestEventLogger>()
                .As<IEventLogger>()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IEventPublisher>();
                var logger = scope.Resolve<IEventLogger>() as TestEventLogger;

                var domainEvent = new Test1Event();

                await publisher.RaiseAsync(domainEvent);

                logger.LoggedEvents.Should()
                    .HaveCount(1)
                    .And.Contain(domainEvent);
            }
        }

        [Fact]
        public async Task Raise_SingleHandler_IsRaised()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<EventHandlingModule>();
            builder.RegisterEventHandlers(typeof(WasCalledVerifiableEvent).GetTypeInfo().Assembly);

            /// Tracker that helps the handler (registered in previous line) determine if it was called.
            builder.RegisterType<WasCalledTracker>()
                .AsSelf()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IEventPublisher>();
                var handler = scope.Resolve<IEnumerable<IEventHandler>>()
                    .OfType<WasCalledVerifiableHandler>()
                    .Single();

                var domainEvent = new WasCalledVerifiableEvent();

                await publisher.RaiseAsync(domainEvent);

                var result = handler.WasEverCalled;

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_SingleHandler_StartIsLogged()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<EventHandlingModule>();
            builder.RegisterEventHandlers(typeof(Test1EventHandler).GetTypeInfo().Assembly);
            builder.RegisterType<VerifiableEventHandlerLogger<Test1EventHandler>>()
                .As<IEventHandlerLogger>()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IEventPublisher>();
                var handlerLogger = scope.Resolve<IEventHandlerLogger>() as IVerifiableEventHandlerLogger;

                var domainEvent = new Test1Event();

                await publisher.RaiseAsync(domainEvent);

                var result = handlerLogger.StartCalled;

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_SingleHandler_CompleteIsLogged()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<EventHandlingModule>();
            builder.RegisterEventHandlers(typeof(Test1EventHandler).GetTypeInfo().Assembly);

            builder.RegisterType<VerifiableEventHandlerLogger<Test1EventHandler>>()
                .As<IEventHandlerLogger>()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IEventPublisher>();
                var handlerLogger = scope.Resolve<IEventHandlerLogger>() as IVerifiableEventHandlerLogger;

                var domainEvent = new Test1Event();

                await publisher.RaiseAsync(domainEvent);

                var result = handlerLogger.CompleteCalled;

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_HandlerRaisesSecondEvent_BothLogged()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<EventHandlingModule>();
            builder.RegisterEventHandlers(typeof(EventWithHandlerRegistered).GetTypeInfo().Assembly);

            builder.RegisterType<TestEventLogger>()
                .As<IEventLogger>()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IEventPublisher>();
                var logger = scope.Resolve<IEventLogger>() as TestEventLogger;

                var originalDomainEvent = new EventWithHandlerRegistered();

                await publisher.RaiseAsync(originalDomainEvent);

                logger.LoggedEvents.Should()
                    .HaveCount(2)
                    .And.Contain(originalDomainEvent)
                    .And.Match(a => a.OfType<EventNoHandlerRegistered>().Any());
            }
        }
    }
}
