using System;
using System.Threading.Tasks;

namespace Mendham.Events.DependencyInjection.TestObjects
{
    public sealed class WasCalledVerifiableHandler : IEventHandler<WasCalledVerifiableEvent>
    {
        public WasCalledVerifiableHandler()
        {
            WasEverCalled = false;
        }

        public bool WasEverCalled { get; private set; }

        public Task HandleAsync(WasCalledVerifiableEvent domainEvent)
        {
            WasEverCalled = true;
            return Task.FromResult(0);
        }
    }

    public sealed class WasCalledVerifiableEvent : Event
    { }

    public class WasCalledVerifiableHandlerLogger : IEventHandlerLogger
    {
        public bool StartCalled { get; private set; }
        public bool CompleteCalled { get; private set; }

        public WasCalledVerifiableHandlerLogger()
        {
            StartCalled = false;
            CompleteCalled = false;
        }

        void IEventHandlerLogger.LogEventHandlerStart(Type handlerType, IEvent eventRaised)
        {
            if (handlerType.Equals(typeof(WasCalledVerifiableHandler)))
            {
                StartCalled = true;
            }
        }

        void IEventHandlerLogger.LogEventHandlerComplete(Type handlerType, IEvent eventRaised)
        {
            if (handlerType.Equals(typeof(WasCalledVerifiableHandler)))
            {
                CompleteCalled = true;
            }
        }

        void IEventHandlerLogger.LogEventHandlerError(Type handlerType, IEvent eventRaised, Exception exception)
        {
            throw new NotImplementedException("Can't be tested for this handler... there is no way to throw");
        }
    }
}
