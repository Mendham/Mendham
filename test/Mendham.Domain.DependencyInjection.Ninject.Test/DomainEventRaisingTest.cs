using FluentAssertions;
using Mendham.Domain.DependencyInjection.Ninject.Test.TestObjects;
using Mendham.Domain.Events;
using Ninject;
using System;
using System.Collections.Generic;
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
                kernel.RegisterDomainEventHandlers(GetType().Assembly);
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
                kernel.RegisterDomainEventHandlers(GetType().Assembly);

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
        public async Task Raise_HandlerLogsSecondEvent_BothLogged()
        {
            using (var kernel = new StandardKernel(new DomainEventHandlingModule()))
            {
                kernel.RegisterDomainEventHandlers(GetType().Assembly);
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
    }
}
