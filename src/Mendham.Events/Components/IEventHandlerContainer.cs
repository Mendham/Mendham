using System.Collections.Generic;

namespace Mendham.Events.Components
{
    public interface IEventHandlerContainer
    {
        IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>() where TEvent : IEvent;
    }
}
