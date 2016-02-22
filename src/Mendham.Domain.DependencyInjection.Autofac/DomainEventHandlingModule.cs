using Autofac;
using Mendham.Domain.Events;
using Mendham.Domain.Events.Components;

namespace Mendham.Domain.DependencyInjection.Autofac
{
    public class DomainEventHandlingModule : Module
    {
		protected override void Load(ContainerBuilder builder)
		{
            builder.RegisterType<DefaultDomainEventHandlerContainer>()
                .As<IDomainEventHandlerContainer>()
                .SingleInstance();

            builder.RegisterType<DomainEventHandlerProcessor>()
                .As<IDomainEventHandlerProcessor>()
                .SingleInstance();

            builder.RegisterType<DomainEventPublisherComponents>()
                .As<IDomainEventPublisherComponents>()
                .SingleInstance();

            builder.RegisterType<DomainEventPublisher>()
                .As<IDomainEventPublisher>()
                .SingleInstance();
        }
	}
}