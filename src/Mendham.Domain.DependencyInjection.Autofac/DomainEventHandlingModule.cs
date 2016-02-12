using Autofac;
using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.Autofac
{
    public class DomainEventHandlingModule : Module
    {
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<DomainEventHandlerContainer>()
				.As<IDomainEventHandlerContainer>()
				.SingleInstance();

            builder.RegisterType<DomainEventLoggerContainer>()
                .As<IDomainEventLoggerContainer>()
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