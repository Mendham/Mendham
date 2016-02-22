using Mendham.Domain.Events;
using Mendham.Domain.Events.Components;
using Ninject;
using Ninject.Modules;
using System.Collections.Generic;

namespace Mendham.Domain.DependencyInjection.Ninject
{
    public class DomainEventHandlingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDomainEventHandlerContainer>()
                .ToMethod(ctx => new DefaultDomainEventHandlerContainer(() => ctx.Kernel.GetAll<IDomainEventHandler>()))
                .InSingletonScope();

            Bind<IDomainEventHandlerProcessor>()
                .To<DomainEventHandlerProcessor>()
                .InSingletonScope();

            Bind<IDomainEventPublisherComponents>()
                .To<DomainEventPublisherComponents>()
                .InSingletonScope();

            Bind<IDomainEventPublisher>()
                .ToMethod(ctx => new DomainEventPublisher(() => ctx.Kernel.Get<IDomainEventPublisherComponents>()))
                .InSingletonScope();
        }
    }
}
