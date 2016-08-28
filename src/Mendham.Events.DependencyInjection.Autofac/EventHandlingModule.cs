using Autofac;
using Mendham.Events;
using Mendham.Events.Components;

namespace Mendham.DependencyInjection.Autofac
{
    public class EventHandlingModule : Module
    {
		protected override void Load(ContainerBuilder builder)
		{
            builder.RegisterType<DefaultEventHandlerContainer>()
                .As<IEventHandlerContainer>()
                .InstancePerDependency();

            builder.RegisterType<EventLoggerProcessor>()
                .As<IEventLoggerProcessor>()
                .InstancePerDependency();

            builder.RegisterType<EventHandlerProcessor>()
                .As<IEventHandlerProcessor>()
                .InstancePerDependency();

            builder.RegisterType<EventPublisherComponents>()
                .As<IEventPublisherComponents>()
                .InstancePerDependency();

            builder.RegisterType<EventPublisher>()
                .As<IEventPublisher>()
                .InstancePerDependency();
        }
	}
}