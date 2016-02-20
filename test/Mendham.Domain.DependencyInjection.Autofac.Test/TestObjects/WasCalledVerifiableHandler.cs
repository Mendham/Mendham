using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.Autofac.Test.TestObjects
{
    public sealed class WasCalledVerifiableHandler : DomainEventHandler<WasCalledVerifiableEvent>
    {
        public WasCalledVerifiableHandler()
        {
            this.WasEverCalled = false;
        }

        public bool WasEverCalled { get; private set; }

        public override Task HandleAsync(WasCalledVerifiableEvent domainEvent)
        {
            WasEverCalled = true;
            return Task.FromResult(0);
        }
    }

    public sealed class WasCalledVerifiableEvent : DomainEvent
    { }
}
