using System;

namespace Mendham.Domain.Events.Components
{
    /// <summary>
    /// Interface used to get the type of the underlying handler within a DomainEventHandlerWrapper
    /// </summary>
    public interface IDomainEventHandlerWrapper : IDomainEventHandler
    {
        Type GetBaseHandlerType();
    }
}
