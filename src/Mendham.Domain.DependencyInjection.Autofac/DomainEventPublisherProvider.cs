using Autofac;
using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.Autofac
{
    internal class DomainEventPublisherProvider : IDomainEventPublisherProvider
    {
        private readonly Func<IDomainEventPublisher> domainEventPublisherProvider;

        public DomainEventPublisherProvider(IComponentContext componentContext)
        {
            this.domainEventPublisherProvider = componentContext.Resolve<Func<IDomainEventPublisher>>();
        }

        private IDomainEventPublisher _publisher;

        public IDomainEventPublisher GetPublisher()
        {
            if (_publisher == default(IDomainEventPublisher))
            {
                _publisher = domainEventPublisherProvider();
            }

            return _publisher;
        }
    }
}