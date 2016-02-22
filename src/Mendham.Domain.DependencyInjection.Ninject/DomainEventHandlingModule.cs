using Mendham.Domain.Events;
using Mendham.Domain.Events.Components;
using Ninject;
using Ninject.Modules;

namespace Mendham.Domain.DependencyInjection.Ninject
{
    public class DomainEventHandlingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDomainEventHandlerContainer>()
                .ToMethod(ctx => new DefaultDomainEventHandlerContainer(() => ctx.Kernel.GetAll<IDomainEventHandler>()))
                .InSingletonScope();

            Bind<IDomainEventLoggerProcessor>()
                .ToMethod(ctx => new DomainEventLoggerProcessor(() => ctx.Kernel.GetAll<IDomainEventLogger>()))
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
