using System;
using System.Threading.Tasks;

namespace Mendham.Events.Components
{
    /// <summary>
    /// When a base event handler must be passed in an enumerable, because the generic type does not match,
    /// handlers for base types must be wrapped in an handler of the derived type. This class does this.
    /// </summary>
    /// <typeparam name="TBaseEvent">Base event type (the type the handler is set to process)</typeparam>
    /// <typeparam name="TDerivedEvent">Derived event (the type of event that was raised)</typeparam>
    internal class EventHandlerWrapper<TBaseEvent, TDerivedEvent> : IEventHandler<TDerivedEvent>, IEventHandlerWrapper
        where TBaseEvent : IEvent
        where TDerivedEvent : TBaseEvent
    {
        private readonly IEventHandler<TBaseEvent> _eventHandler;

        public EventHandlerWrapper(IEventHandler<TBaseEvent> eventHandler)
        {
            eventHandler.VerifyArgumentNotDefaultValue(nameof(eventHandler));

            _eventHandler = eventHandler;
        }

        Task IEventHandler<TDerivedEvent>.HandleAsync(TDerivedEvent eventRaised)
        {
            return _eventHandler.HandleAsync(eventRaised);
        }

        Type IEventHandlerWrapper.GetBaseHandlerType()
        {
            return _eventHandler.GetType();
        }
    }
}
