
using Mendham.Domain.Events;
using Ninject;
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
            Bind<IDomainEventPublisher>()
                .To<DomainEventPublisher>()
                .InSingletonScope();

            Bind<IDomainEventHandlerContainer>()
                .To<DomainEventHandlerContainer>()
                .InSingletonScope();

            Bind<IDomainEventLoggerContainer>()
                .To<DomainEventLoggerContainer>()
                .InSingletonScope();

            Bind<IDomainEventPublisherProvider>()
                .To<DomainEventPublisherProvider>()
                .InSingletonScope();
        }
    }
}
