
using Mendham.Domain.Events;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.Ninject
{
    public class DomainEventHandlingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDomainEventHandlerContainer>()
                .To<DomainEventHandlerContainer>()
                .InSingletonScope();

            Bind<IDomainEventLoggerContainer>()
                .To<DomainEventLoggerContainer>()
                .InSingletonScope();

            Bind<IDomainEventPublisherComponents>()
                .To<DomainEventPublisherComponents>()
                .InSingletonScope();

            Bind<IDomainEventPublisher>()
                .ToMethod(CreateDomainEventPublisher)
                .InSingletonScope();
        }

        private static IDomainEventPublisher CreateDomainEventPublisher(IContext context)
        {
            Func<IDomainEventPublisherComponents> domainEventPublisherContainerFactory = () => 
                context.Kernel.Get<IDomainEventPublisherComponents>();

            return new DomainEventPublisher(domainEventPublisherContainerFactory);
        }
    }
}
