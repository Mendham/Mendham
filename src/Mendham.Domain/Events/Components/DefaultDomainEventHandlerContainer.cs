using System;
using System.Collections.Generic;

namespace Mendham.Events.Components
{
    public class DefaultEventHandlerContainer : IEventHandlerContainer
    {
        private readonly Func<IEnumerable<IEventHandler>> _eventHandlersFactory;

        public DefaultEventHandlerContainer(Func<IEnumerable<IEventHandler>> eventHandlersFactory)
        {
            this._eventHandlersFactory = eventHandlersFactory;
        }

        public IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>() where TEvent : IEvent
        {
            return _eventHandlersFactory()
                .SelectHandlersForEvent<TEvent>();
        }
    }
}
