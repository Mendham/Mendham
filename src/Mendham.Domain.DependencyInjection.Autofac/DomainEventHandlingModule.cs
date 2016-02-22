using Autofac;
using Mendham.Domain.Events;
using Mendham.Domain.Events.Components;
using System;

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

            builder.Register<IDomainEventPublisher>(c =>
            {
                Func<IDomainEventPublisherComponents> containerFactory =
                    c.Resolve<Func<IDomainEventPublisherComponents>>();

                return new DomainEventPublisher(containerFactory);
            }).SingleInstance();
		}
	}
}