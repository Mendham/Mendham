using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events.Components
{
    /// <summary>
    /// When a base domain event handler must be passed in an enumerable, because the generic type does not match,
    /// handlers for base types must be wrapped in an handler of the derived type. This class does this.
    /// </summary>
    /// <typeparam name="TBaseDomainEvent">Base domain event type (the type the handler is set to process)</typeparam>
    /// <typeparam name="TDerivedDomainEvent">Derived domain event (the type of domain event that was raised)</typeparam>
    internal class DomainEventHandlerWrapper<TBaseDomainEvent, TDerivedDomainEvent> : IDomainEventHandler<TDerivedDomainEvent>, IDomainEventHandlerWrapper
        where TBaseDomainEvent : IDomainEvent
        where TDerivedDomainEvent : TBaseDomainEvent
    {
        private readonly IDomainEventHandler<TBaseDomainEvent> domainEventHandler;

        public DomainEventHandlerWrapper(IDomainEventHandler<TBaseDomainEvent> domainEventHandler)
        {
            domainEventHandler.VerifyArgumentNotDefaultValue(nameof(domainEventHandler));

            this.domainEventHandler = domainEventHandler;
        }

        Task IDomainEventHandler<TDerivedDomainEvent>.HandleAsync(TDerivedDomainEvent domainEvent)
        {
            return this.domainEventHandler.HandleAsync(domainEvent);
        }

        Type IDomainEventHandlerWrapper.GetBaseHandlerType()
        {
            return this.domainEventHandler.GetType();
        }
    }
}
