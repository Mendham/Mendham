using Mendham.Events;
using System.Collections.Generic;
using System.Linq;

namespace Mendham.Domain.DependencyInjection.TestObjects
{
    public class TestEventLogger : IEventLogger
    {
        private readonly List<IEvent> _loggedEvents;

        public TestEventLogger()
        {
            _loggedEvents = new List<IEvent>();
        }

        public void LogEvent(IEvent eventRaised)
        {
            _loggedEvents.Add(eventRaised);
        }

        public List<IEvent> LoggedEvents
        {
            get { return _loggedEvents.ToList(); }
        }
    }
}
