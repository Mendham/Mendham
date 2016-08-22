using FluentAssertions;
using Mendham.Domain.DependencyInjection.ComplexDomainGraph;
using Mendham.Domain.DependencyInjection.Ninject.Test.TestObjects;
using Mendham.Events;
using Mendham.Events.DependencyInjection.TestObjects;
using Ninject;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Ninject.Test
{
    public class DomainEventRaisingTest
    {
        [Fact]
        public async Task Raise_SingleEvent_IsLogged()
        {
            using (var kernel = new StandardKernel(new EventHandlingModule()))
            {
                kernel.RegisterEventHandlers(typeof(TestEventLogger).GetTypeInfo().Assembly);
                kernel.Bind<IEventLogger>().To<TestEventLogger>().InSingletonScope();

                var publisher = kernel.Get<IEventPublisher>();
                var logger = kernel.Get<IEventLogger>() as TestEventLogger;

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
            using (var kernel = new StandardKernel(new EventHandlingModule()))
            {
                kernel.RegisterEventHandlers(typeof(WasCalledVerifiableEvent).GetTypeInfo().Assembly);

                var publisher = kernel.Get<IEventPublisher>();
                var handler = kernel.GetAll<IEventHandler>()
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
            using (var kernel = new StandardKernel(new EventHandlingModule()))
            {
                kernel.RegisterEventHandlers(typeof(WasCalledVerifiableEvent).GetTypeInfo().Assembly);
                kernel.Bind<IEventHandlerLogger>()
                    .To<WasCalledVerifiableHandlerLogger>()
                    .InSingletonScope();

                var publisher = kernel.Get<IEventPublisher>();
                var handlerLogger = kernel.Get<IEventHandlerLogger>() as WasCalledVerifiableHandlerLogger;

                var domainEvent = new WasCalledVerifiableEvent();

                await publisher.RaiseAsync(domainEvent);

                var result = handlerLogger.StartCalled;

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_SingleHandler_CompleteIsLogged()
        {
            using (var kernel = new StandardKernel(new EventHandlingModule()))
            {
                kernel.RegisterEventHandlers(typeof(WasCalledVerifiableEvent).GetTypeInfo().Assembly);
                kernel.Bind<IEventHandlerLogger>()
                    .To<WasCalledVerifiableHandlerLogger>()
                    .InSingletonScope();

                var publisher = kernel.Get<IEventPublisher>();
                var handlerLogger = kernel.Get<IEventHandlerLogger>() as WasCalledVerifiableHandlerLogger;

                var domainEvent = new WasCalledVerifiableEvent();

                await publisher.RaiseAsync(domainEvent);

                var result = handlerLogger.CompleteCalled;

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_HandlerLogsSecondEvent_BothLogged()
        {
            using (var kernel = new StandardKernel(new EventHandlingModule()))
            {
                kernel.RegisterEventHandlers(typeof(EventWithHandlerRegistered).GetTypeInfo().Assembly);
                kernel.Bind<IEventLogger>().To<TestEventLogger>().InSingletonScope();

                var publisher = kernel.Get<IEventPublisher>();
                var logger = kernel.Get<IEventLogger>() as TestEventLogger;

                var originalDomainEvent = new EventWithHandlerRegistered();

                await publisher.RaiseAsync(originalDomainEvent);

                logger.LoggedEvents.Should()
                    .HaveCount(2)
                    .And.Contain(originalDomainEvent)
                    .And.Match(a => a.OfType<EventNoHandlerRegistered>().Any());
            }
        }

        [Fact]
        public async Task Raise_ComplexDomainGraph_ReturnsTrueAfterProcessing()
        {
            // The purpose of this test is to take an action on a complex graph to make sure the container 
            // does not throw a circular dependency error

            using (var kernel = new StandardKernel(new EventHandlingModule()))
            {
                kernel.RegisterEventHandlers(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);
                kernel.RegisterDomainFacades(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);
                kernel.Bind<IHasCircularHandlerService>().To<HasCircularHandlerService>();
                kernel.Bind<ICountService>().To<CountService>().InSingletonScope();
                kernel.Bind<IOtherService>().To<OtherService>();
                kernel.Bind<IEntityCreationService>().To<EntityCreationService>();
                kernel.Bind<IEntityFactory>().To<ComplexGraphEntityFactory>();

                var sut = kernel.Get<IHasCircularHandlerService>();

                var result = await sut.StartAsync();

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_ComplexDomainGraphMultiple_ReturnsTrueAfterProcessing()
        {
            // The purpose of this test is to take an action on a complex graph to make sure the container 
            // does not throw a circular dependency error

            using (var kernel = new StandardKernel(new EventHandlingModule()))
            {
                kernel.RegisterEventHandlers(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);
                kernel.RegisterDomainFacades(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);
                kernel.Bind<IHasCircularHandlerService>().To<HasCircularHandlerService>();
                kernel.Bind<ICountService>().To<CountService>().InSingletonScope();
                kernel.Bind<IOtherService>().To<OtherService>();
                kernel.Bind<IEntityCreationService>().To<EntityCreationService>();
                kernel.Bind<IEntityFactory>().To<ComplexGraphEntityFactory>();

                var sut = kernel.Get<IHasCircularHandlerService>();

                var result = await Task.WhenAll(Enumerable.Range(1, 10)
                    .Select(a => sut.StartAsync()).ToList());

                result.Should().NotBeEmpty()
                    .And.NotContain(false);
            }
        }
    }
}
