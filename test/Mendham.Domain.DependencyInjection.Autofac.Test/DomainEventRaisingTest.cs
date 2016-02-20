using Autofac;
using FluentAssertions;
using Mendham.Domain.DependencyInjection.Autofac.Test.TestObjects;
using Mendham.Domain.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Autofac.Test
{
    public class DomainEventRaisingTest
    {
        [Fact]
        public async Task Raise_SingleEvent_IsLogged()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterDomainEventHandlers(GetType().Assembly);

            builder.RegisterType<TestDomainEventLogger>()
                .As<IDomainEventLogger>()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IDomainEventPublisher>();
                var logger = scope.Resolve<IDomainEventLogger>() as TestDomainEventLogger;

                var domainEvent = new Test1DomainEvent();

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

            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterDomainEventHandlers(GetType().Assembly);

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IDomainEventPublisher>();
                var handler = scope.Resolve<IEnumerable<IDomainEventHandler>>()
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

            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterDomainEventHandlers(GetType().Assembly);

            builder.RegisterType<WasCalledVerifiableHandlerLogger>()
                .As<IDomainEventHandlerLogger>()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IDomainEventPublisher>();
                var handlerLogger = scope.Resolve<IDomainEventHandlerLogger>() as WasCalledVerifiableHandlerLogger;

                var domainEvent = new WasCalledVerifiableEvent();

                await publisher.RaiseAsync(domainEvent);

                var result = handlerLogger.StartCalled;

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_SingleHandler_CompleteIsLogged()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterDomainEventHandlers(GetType().Assembly);

            builder.RegisterType<WasCalledVerifiableHandlerLogger>()
                .As<IDomainEventHandlerLogger>()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IDomainEventPublisher>();
                var handlerLogger = scope.Resolve<IDomainEventHandlerLogger>() as WasCalledVerifiableHandlerLogger; 

                var domainEvent = new WasCalledVerifiableEvent();

                await publisher.RaiseAsync(domainEvent);

                var result = handlerLogger.CompleteCalled;

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_HandlerRaisesSecondEvent_BothLogged()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterDomainEventHandlers(GetType().Assembly);

            builder.RegisterType<TestDomainEventLogger>()
                .As<IDomainEventLogger>()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IDomainEventPublisher>();
                var logger = scope.Resolve<IDomainEventLogger>() as TestDomainEventLogger;

                var originalDomainEvent = new DomainEventWithHandlerRegistered();

                await publisher.RaiseAsync(originalDomainEvent);

                logger.LoggedEvents.Should()
                    .HaveCount(2)
                    .And.Contain(originalDomainEvent)
                    .And.Match(a => a.OfType<DomainEventNoHandlerRegistered>().Any());
            }
        }
    }
}
