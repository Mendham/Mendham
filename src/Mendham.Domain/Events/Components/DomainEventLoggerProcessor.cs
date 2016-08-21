using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Events.Components
{
    public class EventLoggerProcessor : IEventLoggerProcessor
    {
        private readonly Func<IEnumerable<IEventLogger>> loggersFactory;

        public EventLoggerProcessor(Func<IEnumerable<IEventLogger>> loggersFactory)
        {
            this.loggersFactory = loggersFactory;
        }

        public void LogEvent(IEvent eventRaised)
        {
            foreach (var logger in loggersFactory())
            {
                logger.LogEvent(eventRaised);
            }
        }
    }
}
