using Mendham.Events;
using Mendham.Events.Components;
using Ninject;
using Ninject.Modules;

namespace Mendham.Domain.DependencyInjection.Ninject
{
    public class EventHandlingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEventHandlerContainer>()
                .ToMethod(ctx => new DefaultEventHandlerContainer(() => ctx.Kernel.GetAll<IEventHandler>()))
                .InSingletonScope();

            Bind<IEventLoggerProcessor>()
                .ToMethod(ctx => new EventLoggerProcessor(() => ctx.Kernel.GetAll<IEventLogger>()))
                .InSingletonScope();

            Bind<IEventHandlerProcessor>()
                .To<EventHandlerProcessor>()
                .InSingletonScope();

            Bind<IEventPublisherComponents>()
                .To<EventPublisherComponents>()
                .InSingletonScope();

            Bind<IEventPublisher>()
                .ToMethod(ctx => new EventPublisher(() => ctx.Kernel.Get<IEventPublisherComponents>()))
                .InSingletonScope();
        }
    }
}
