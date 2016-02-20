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

    public class WasCalledVerifiableHandlerLogger : IDomainEventHandlerLogger
    {
        public bool StartCalled { get; private set; }
        public bool CompleteCalled { get; private set; }

        public WasCalledVerifiableHandlerLogger()
        {
            StartCalled = false;
            CompleteCalled = false;
        }

        void IDomainEventHandlerLogger.LogDomainEventHandlerStart(Type handlerType, IDomainEvent domainEvent)
        {
            if (handlerType.Equals(typeof(WasCalledVerifiableHandler)))
            {
                StartCalled = true;
            }
        }

        void IDomainEventHandlerLogger.LogDomainEventHandlerComplete(Type handlerType, IDomainEvent domainEvent)
        {
            if (handlerType.Equals(typeof(WasCalledVerifiableHandler)))
            {
                CompleteCalled = true;
            }
        }

        void IDomainEventHandlerLogger.LogDomainEventHandlerError(Type handlerType, IDomainEvent domainEvent, Exception exception)
        {
            throw new NotImplementedException("Can't be tested for this handler... there is no way to throw");
        }
    }
}
