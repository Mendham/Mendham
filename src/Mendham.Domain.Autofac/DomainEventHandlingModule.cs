using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Mendham.Domain.Events;

namespace Mendham.Domain.Autofac
{
    public class DomainEventHandlingModule : Module
    {
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<DomainEventPublisher>()
				.As<IDomainEventPublisher>()
				.SingleInstance();

			builder.RegisterType<DomainEventHandlerContainer>()
				.As<IDomainEventHandlerContainer>()
				.SingleInstance();

            builder.Register<IDomainEventPublisherProvider>(c => new DomainEventPublisherProvider(c))
                .SingleInstance();
		}
	}
}