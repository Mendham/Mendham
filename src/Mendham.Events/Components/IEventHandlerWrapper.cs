using System;

namespace Mendham.Events.Components
{
    /// <summary>
    /// Interface used to get the type of the underlying handler within a 
    /// <see cref="EventHandlerWrapper{TBaseEvent, TDerivedEvent}"/> 
    /// </summary>
    public interface IEventHandlerWrapper : IEventHandler
    {
        Type GetBaseHandlerType();
    }
}
