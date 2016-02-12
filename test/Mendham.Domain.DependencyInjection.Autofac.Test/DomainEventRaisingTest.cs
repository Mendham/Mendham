using Autofac;
using FluentAssertions;
using Mendham.Domain.DependencyInjection.Autofac.Test.TestObjects;
using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Autofac.Test
{
    public class DomainEventRaisingTest
    {
        [Fact]
        public async Task Raise_HandlerLogsSecondEvent_BothLogged()
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
