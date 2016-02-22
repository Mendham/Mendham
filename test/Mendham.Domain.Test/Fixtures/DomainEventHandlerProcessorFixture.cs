using Mendham.Domain.Events;
using Mendham.Domain.Events.Components;
using Mendham.Domain.Test.TestObjects.Events;
using Mendham.Testing;
using Moq;
using System.Collections.Generic;

namespace Mendham.Domain.Test.Fixtures
{
    public class DomainEventHandlerProcessorFixture : Fixture<DomainEventHandlerProcessor>
    {
        public IDomainEventHandler<TestDomainEvent> DomainEventHandler1 { get; set; }
        public IDomainEventHandler<TestDomainEvent> DomainEventHandler2 { get; set; }

        public IDomainEventHandlerLogger DomainEventHandlerLogger { get; set; }

        public TestDomainEvent DomainEvent { get; set; }

        public override DomainEventHandlerProcessor CreateSut()
        {
            return new DomainEventHandlerProcessor(DomainEventHandlerLogger.AsSingleItemEnumerable());
        }

        public override void ResetFixture()
        {
            base.ResetFixture();

            DomainEventHandlerLogger = Mock.Of<IDomainEventHandlerLogger>();

            DomainEventHandler1 = Mock.Of<IDomainEventHandler<TestDomainEvent>>();
            DomainEventHandler2 = Mock.Of<AltTestDomainEventHandlerInterface>();

            DomainEvent = new TestDomainEvent();
        }

        public IEnumerable<IDomainEventHandler<TestDomainEvent>> GetAllDomainEventHandlers()
        {
            yield return DomainEventHandler1;
            yield return DomainEventHandler2;
        }

        public IEnumerable<IDomainEventHandler<TestDomainEvent>> GetFirstDomainEventHandlerOnly()
        {
            yield return DomainEventHandler1;
        }

        // This is just done as a trick to make mock think the second interface is a different type
        public interface AltTestDomainEventHandlerInterface : IDomainEventHandler<TestDomainEvent>
        { }
    }
}