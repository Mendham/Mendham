using Mendham.Domain.Events;
using Mendham.Events;

namespace Mendham.Domain.DependencyInjection.TestObjects
{
    public class DerivedTestEntityWithDomainFacade : AbstractTestEntityWithDomainFacade
    {
        private readonly IDerivedFacade _derivedFacade;

        public DerivedTestEntityWithDomainFacade(int id, IDerivedFacade facade)
            : base(id, facade)
        {
            _derivedFacade = facade;
        }

        public bool HasValidDerivedFacade()
        {
            return _derivedFacade != null && _derivedFacade.HasDomainEventPublisher();
        }

        public interface IDerivedFacade : IBaseFacade
        {
            bool HasValidDerivedFacade();
        }

        public class DerivedFacade : BaseFacade, IDerivedFacade
        {
            private readonly bool _eventPublisherHasValue;

            public DerivedFacade(IEventPublisher eventPublisher)
                : base(eventPublisher)
            {
                _eventPublisherHasValue = eventPublisher != null;
            }

            public bool HasValidDerivedFacade()
            {
                return _eventPublisherHasValue;
            }
        }
    }
}
