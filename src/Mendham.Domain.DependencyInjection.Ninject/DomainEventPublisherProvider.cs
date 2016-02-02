using Mendham.Domain.Events;
using Ninject;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.Ninject
{
    internal class DomainEventPublisherProvider : IDomainEventPublisherProvider
    {
        private readonly IResolutionRoot resolutionRoot;

        public DomainEventPublisherProvider(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }

        public IDomainEventPublisher GetPublisher()
        {
            return resolutionRoot.Get<IDomainEventPublisher>();
        }
    }
}
