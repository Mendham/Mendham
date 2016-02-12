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
