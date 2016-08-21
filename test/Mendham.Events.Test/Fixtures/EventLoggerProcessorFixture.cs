using Mendham.Events.Components;
using Mendham.Events.Test.TestObjects;
using Mendham.Testing;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Mendham.Events.Test.Fixtures
{
    public class EventLoggerProcessorFixture : Fixture<EventLoggerProcessor>
    {
        public IEventLogger EventLogger1 { get; set; }
        public IEventLogger EventLogger2 { get; set; }

        private List<IEventLogger> _loggers;

        public EventLoggerProcessorFixture()
        {
            _loggers = new List<IEventLogger>();
        }

        public TestEvent Event { get; set; }

        public override EventLoggerProcessor CreateSut()
        {
            var loggersAtCreation = _loggers.ToList();
            return new EventLoggerProcessor(() => loggersAtCreation);
        }

        public void AddLogger(IEventLogger logger)
        {
            _loggers.Add(logger);
        }

        public override void ResetFixture()
        {
            base.ResetFixture();

            _loggers.Clear();

            EventLogger1 = Mock.Of<IEventLogger>();
            EventLogger2 = Mock.Of<IEventLogger>();
        }
    }
}
