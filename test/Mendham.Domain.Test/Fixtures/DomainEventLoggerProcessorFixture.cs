using Mendham.Domain.Events;
using Mendham.Domain.Events.Components;
using Mendham.Domain.Test.TestObjects.Events;
using Mendham.Testing;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Mendham.Domain.Test.Fixtures
{
    public class DomainEventLoggerProcessorFixture : Fixture<DomainEventLoggerProcessor>
    {
        public IDomainEventLogger DomainEventLogger1 { get; set; }
        public IDomainEventLogger DomainEventLogger2 { get; set; }

        private List<IDomainEventLogger> loggers;

        public DomainEventLoggerProcessorFixture()
        {
            loggers = new List<IDomainEventLogger>();
        }

        public TestDomainEvent DomainEvent { get; set; }

        public override DomainEventLoggerProcessor CreateSut()
        {
            var loggersAtCreation = loggers.ToList();
            return new DomainEventLoggerProcessor(() => loggersAtCreation);
        }

        public void AddLogger(IDomainEventLogger logger)
        {
            loggers.Add(logger);
        }

        public override void ResetFixture()
        {
            base.ResetFixture();

            loggers.Clear();

            DomainEventLogger1 = Mock.Of<IDomainEventLogger>();
            DomainEventLogger2 = Mock.Of<IDomainEventLogger>();
        }
    }
}
