using Autofac;
using Mendham.Events.Components;

namespace Mendham.Events.DependencyInjection.Autofac
{
    public class EventHandlingModule : Module
    {
		protected override void Load(ContainerBuilder builder)
		{
            builder.RegisterType<DefaultEventHandlerContainer>()
                .As<IEventHandlerContainer>()
                .SingleInstance();

            builder.RegisterType<EventHandlerProcessor>()
                .As<IEventHandlerProcessor>()
                .SingleInstance();

            builder.RegisterType<EventLoggerProcessor>()
                .As<IEventLoggerProcessor>()
                .SingleInstance();

            builder.RegisterType<EventPublisherComponents>()
                .As<IEventPublisherComponents>()
                .SingleInstance();

            builder.RegisterType<EventPublisher>()
                .As<IEventPublisher>()
                .SingleInstance();
        }
	}
}