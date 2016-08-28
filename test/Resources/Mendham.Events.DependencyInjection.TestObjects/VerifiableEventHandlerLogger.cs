using System;

namespace Mendham.Events.DependencyInjection.TestObjects
{
    public class VerifiableEventHandlerLogger<TEventHandler> : IEventHandlerLogger, IVerifiableEventHandlerLogger
        where TEventHandler : IEventHandler
    {
        public bool StartCalled { get; private set; }
        public bool CompleteCalled { get; private set; }

        public VerifiableEventHandlerLogger()
        {
            StartCalled = false;
            CompleteCalled = false;
        }

        void IEventHandlerLogger.LogEventHandlerStart(Type handlerType, IEvent eventRaised)
        {
            if (handlerType.Equals(typeof(TEventHandler)))
            {
                StartCalled = true;
            }
        }

        void IEventHandlerLogger.LogEventHandlerComplete(Type handlerType, IEvent eventRaised)
        {
            if (handlerType.Equals(typeof(TEventHandler)))
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
