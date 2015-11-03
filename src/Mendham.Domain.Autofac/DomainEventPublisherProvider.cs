using Autofac;
using Autofac.Core;
using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Autofac
{
    internal class DomainEventPublisherProvider : IDomainEventPublisherProvider
    {
        private readonly Func<IDomainEventPublisher> domainEventPublisherProvider;

        public DomainEventPublisherProvider(IComponentContext componentContext)
        {
            this.domainEventPublisherProvider = componentContext.Resolve<Func<IDomainEventPublisher>>();
        }

        public IDomainEventPublisher GetPublisher()
        {
            return domainEventPublisherProvider();
        }
    }
}