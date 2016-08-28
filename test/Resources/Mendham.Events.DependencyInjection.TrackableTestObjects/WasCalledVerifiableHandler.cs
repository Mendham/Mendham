using System;
using System.Threading.Tasks;

namespace Mendham.Events.DependencyInjection.TrackableTestObjects
{
    public sealed class WasCalledVerifiableHandler : IEventHandler<WasCalledVerifiableEvent>
    {
        private readonly WasCalledTracker _tracker;

        public WasCalledVerifiableHandler(WasCalledTracker tracker)
        {
            _tracker = tracker;
        }

        public bool WasEverCalled
        {
            get
            {
                return _tracker.WasEverCalled;
            }
        }

        public Task HandleAsync(WasCalledVerifiableEvent domainEvent)
        {
            _tracker.CallMade();
            return Task.FromResult(0);
        }
    }

    public class WasCalledTracker
    {
        public bool WasEverCalled { get; private set; } = false;

        public void CallMade()
        {
            WasEverCalled = true;
        }
    }

    public sealed class WasCalledVerifiableEvent : Event
    { }
}
