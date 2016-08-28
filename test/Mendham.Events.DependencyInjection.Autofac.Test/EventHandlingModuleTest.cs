using Autofac;
using FluentAssertions;
using Mendham.DependencyInjection.Autofac;
using Mendham.Events.Components;
using Mendham.Events.DependencyInjection.TestObjects;
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
                var result = sut.Resolve<IEventPublisher>();

                result.Should()
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
                var result = sut.Resolve<IEventHandlerContainer>();

                result.Should()
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
                var result = sut.Resolve<IEventHandlerProcessor>();

                result.Should()
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
                var result = sut.Resolve<IEventLoggerProcessor>();

                result.Should()
                    .NotBeNull()
                    .And.BeOfType<EventLoggerProcessor>();
            }
        }

        [Fact]
        public void EventHandlingModule_RegisterEventPublisherAfterOther_UsesOtherEventPublisher()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<AltEventPublisher>()
                .As<IEventPublisher>();
            builder.RegisterModule<EventHandlingModule>();

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var result = sut.Resolve<IEventPublisher>();

                result.Should()
                    .NotBeNull()
                    .And.BeOfType<AltEventPublisher>();
            }
        }
    }
}
