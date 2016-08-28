using Mendham.Events;
using Mendham.Events.Components;
using Ninject;
using Ninject.Modules;

namespace Mendham.DependencyInjection.Ninject
{
    public class EventHandlingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEventHandlerContainer>()
                .ToMethod(ctx => new DefaultEventHandlerContainer(() => ctx.Kernel.GetAll<IEventHandler>()))
                .InTransientScope();

            Bind<IEventLoggerProcessor>()
                .ToMethod(ctx => new EventLoggerProcessor(() => ctx.Kernel.GetAll<IEventLogger>()))
                .InTransientScope();

            Bind<IEventHandlerProcessor>()
                .To<EventHandlerProcessor>()
                .InTransientScope();

            Bind<IEventPublisherComponents>()
                .To<EventPublisherComponents>()
                .InTransientScope();

            Bind<IEventPublisher>()
                .To<EventPublisher>()
                .InTransientScope();
        }
    }
}
