using Autofac;
using FluentAssertions;
using Mendham.DependencyInjection.Autofac;
using Mendham.Events.Components;
using Xunit;

namespace Mendham.Events.DependencyInjection.Autofac.Test
{
    public class EventHandlingModuleTest
    {
        [Fact]
        public void EventHandlingModule_RegisterEventPublisher_Resolves()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var publisher = sut.Resolve<IEventPublisher>();

                publisher.Should()
                    .NotBeNull()
                    .And.BeOfType<EventPublisher>();
            }
        }

        [Fact]
        public void EventHandlingModule_RegisterEventHandlingContainer_Resolves()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var publisher = sut.Resolve<IEventHandlerContainer>();

                publisher.Should()
                    .NotBeNull()
                    .And.BeOfType<DefaultEventHandlerContainer>();
            }
        }

        [Fact]
        public void EventHandlingModule_RegisterEventHandlingProcessor_Resolves()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var publisher = sut.Resolve<IEventHandlerProcessor>();

                publisher.Should()
                    .NotBeNull()
                    .And.BeOfType<EventHandlerProcessor>();
            }
        }

        [Fact]
        public void EventHandlingModule_RegisterEventLoggerProcessor_Resolves()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var publisher = sut.Resolve<IEventLoggerProcessor>();

                publisher.Should()
                    .NotBeNull()
                    .And.BeOfType<EventLoggerProcessor>();
            }
        }
    }
}
