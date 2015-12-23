
using Mendham.Domain.Events;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Ninject
{
    public class DomainEventHandlingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDomainEventPublisher>()
                .To<DomainEventPublisher>()
                .InSingletonScope();

            Bind<IDomainEventHandlerContainer>()
                .To<DomainEventHandlerContainer>()
                .InSingletonScope();

            Bind<IDomainEventPublisherProvider>()
                .To<DomainEventPublisherProvider>()
                .InSingletonScope();
        }
    }
}
