using FluentAssertions;
using Mendham.Domain.DependencyInjection.ComplexDomainGraph;
using Mendham.Domain.DependencyInjection.Ninject.Test.TestObjects;
using Mendham.Domain.DependencyInjection.TestObjects;
using Mendham.Domain.Events;
using Ninject;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Ninject.Test
{
    public class DomainEventRaisingTest
    {
        [Fact]
        public async Task Raise_SingleEvent_IsLogged()
        {
            using (var kernel = new StandardKernel(new DomainEventHandlingModule()))
            {
                kernel.RegisterDomainEventHandlers(typeof(TestDomainEventLogger).Assembly);
                kernel.Bind<IDomainEventLogger>().To<TestDomainEventLogger>().InSingletonScope();

                var publisher = kernel.Get<IDomainEventPublisher>();
                var logger = kernel.Get<IDomainEventLogger>() as TestDomainEventLogger;

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
            using (var kernel = new StandardKernel(new DomainEventHandlingModule()))
            {
                kernel.RegisterDomainEventHandlers(typeof(WasCalledVerifiableEvent).Assembly);

                var publisher = kernel.Get<IDomainEventPublisher>();
                var handler = kernel.GetAll<IDomainEventHandler>()
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
            using (var kernel = new StandardKernel(new DomainEventHandlingModule()))
            {
                kernel.RegisterDomainEventHandlers(typeof(WasCalledVerifiableEvent).Assembly);
                kernel.Bind<IDomainEventHandlerLogger>()
                    .To<WasCalledVerifiableHandlerLogger>()
                    .InSingletonScope();

                var publisher = kernel.Get<IDomainEventPublisher>();
                var handlerLogger = kernel.Get<IDomainEventHandlerLogger>() as WasCalledVerifiableHandlerLogger;

                var domainEvent = new WasCalledVerifiableEvent();

                await publisher.RaiseAsync(domainEvent);

                var result = handlerLogger.StartCalled;

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_SingleHandler_CompleteIsLogged()
        {
            using (var kernel = new StandardKernel(new DomainEventHandlingModule()))
            {
                kernel.RegisterDomainEventHandlers(typeof(WasCalledVerifiableEvent).Assembly);
                kernel.Bind<IDomainEventHandlerLogger>()
                    .To<WasCalledVerifiableHandlerLogger>()
                    .InSingletonScope();

                var publisher = kernel.Get<IDomainEventPublisher>();
                var handlerLogger = kernel.Get<IDomainEventHandlerLogger>() as WasCalledVerifiableHandlerLogger;

                var domainEvent = new WasCalledVerifiableEvent();

                await publisher.RaiseAsync(domainEvent);

                var result = handlerLogger.CompleteCalled;

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_HandlerLogsSecondEvent_BothLogged()
        {
            using (var kernel = new StandardKernel(new DomainEventHandlingModule()))
            {
                kernel.RegisterDomainEventHandlers(typeof(DomainEventWithHandlerRegistered).Assembly);
                kernel.Bind<IDomainEventLogger>().To<TestDomainEventLogger>().InSingletonScope();

                var publisher = kernel.Get<IDomainEventPublisher>();
                var logger = kernel.Get<IDomainEventLogger>() as TestDomainEventLogger;

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

            using (var kernel = new StandardKernel(new DomainEventHandlingModule()))
            {
                kernel.RegisterDomainEventHandlers(typeof(IHasCircularHandlerService).Assembly);
                kernel.RegisterDomainFacades(typeof(IHasCircularHandlerService).Assembly);
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
    }
}
