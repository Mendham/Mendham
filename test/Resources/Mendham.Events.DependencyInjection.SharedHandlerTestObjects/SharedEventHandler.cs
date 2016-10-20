using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Events.DependencyInjection.SharedHandlerTestObjects
{
    public class SharedEventHandler : IEventHandler<SharedEvent1>, IEventHandler<SharedEvent2>
    {
        private readonly SharedHandlerTracker _tracker;

        public SharedEventHandler(SharedHandlerTracker tracker)
        {
            _tracker = tracker;
        }

        public Task HandleAsync(SharedEvent1 eventToHandle)
        {
            _tracker.Event1Called();
            return Task.FromResult(0);
        }

        public Task HandleAsync(SharedEvent2 eventToHandle)
        {
            _tracker.Event2Called();
            return Task.FromResult(0);
        }
    }
}
