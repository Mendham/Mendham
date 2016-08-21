using Autofac;
using FluentAssertions;
using Mendham.Domain.DependencyInjection.Autofac.Test.TestObjects;
using Mendham.Domain.DependencyInjection.ComplexDomainGraph;
using Mendham.Domain.DependencyInjection.TestObjects;
using Mendham.Events;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            builder.RegisterType<TestEventLogger>()
                .As<IEventLogger>()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IEventPublisher>();
                var logger = scope.Resolve<IEventLogger>() as TestEventLogger;

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
            builder.RegisterEventHandlers(typeof(WasCalledVerifiableEvent).GetTypeInfo().Assembly);

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

            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterEventHandlers(typeof(WasCalledVerifiableEvent).GetTypeInfo().Assembly);

            builder.RegisterType<WasCalledVerifiableHandlerLogger>()
                .As<IEventHandlerLogger>()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IEventPublisher>();
                var handlerLogger = scope.Resolve<IEventHandlerLogger>() as WasCalledVerifiableHandlerLogger;

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
            builder.RegisterEventHandlers(typeof(WasCalledVerifiableEvent).GetTypeInfo().Assembly);

            builder.RegisterType<WasCalledVerifiableHandlerLogger>()
                .As<IEventHandlerLogger>()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IEventPublisher>();
                var handlerLogger = scope.Resolve<IEventHandlerLogger>() as WasCalledVerifiableHandlerLogger; 

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
            builder.RegisterEventHandlers(typeof(DomainEventWithHandlerRegistered).GetTypeInfo().Assembly);

            builder.RegisterType<TestEventLogger>()
                .As<IEventLogger>()
                .SingleInstance();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var publisher = scope.Resolve<IEventPublisher>();
                var logger = scope.Resolve<IEventLogger>() as TestEventLogger;

                var originalDomainEvent = new DomainEventWithHandlerRegistered();

                await publisher.RaiseAsync(originalDomainEvent);

                logger.LoggedEvents.Should()
                    .HaveCount(2)
                    .And.Contain(originalDomainEvent)
                    .And.Match(a => a.OfType<DomainEventNoHandlerRegistered>().Any());
            }
        }

        [Fact]
        public async Task Raise_ComplexDomainGraph_ReturnsTrueAfterProcessing()
        {
            // The purpose of this test is to take an action on a complex graph to make sure the container 
            // does not throw a circular dependency error

            var builder = new ContainerBuilder();

            builder.RegisterModule<DomainEventHandlingModule>();
            builder.RegisterEventHandlers(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);
            builder.RegisterDomainFacades(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);
            builder.RegisterEntities(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);

            builder.RegisterType<HasCircularHandlerService>().As<IHasCircularHandlerService>();
            builder.RegisterType<CountService>().As<ICountService>().SingleInstance();
            builder.RegisterType<OtherService>().As<IOtherService>();
            builder.RegisterType<EntityCreationService>().As<IEntityCreationService>();
            builder.RegisterType<ComplexGraphEntityFactory>().As<IEntityFactory>();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var sut = scope.Resolve<IHasCircularHandlerService>();

                var result = await sut.StartAsync();

                result.Should().BeTrue();
            }
        }
    }
}
