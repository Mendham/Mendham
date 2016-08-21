using Mendham.Events;
using System;

namespace Mendham.Domain.Events
{
    public class DomainEvent : Event, IDomainEvent
    {
        public DomainEvent()
        { }

        public DomainEvent(DateTimeOffset eventTime) : base(eventTime)
        { }
    }
}
