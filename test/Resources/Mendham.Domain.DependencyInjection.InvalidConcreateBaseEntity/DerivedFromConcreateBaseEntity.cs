using Mendham.Events;

namespace Mendham.Domain.DependencyInjection.InvalidConcreateBaseEntity
{
    public class DerivedFromConcreateBaseEntity : ConcreateBaseEntity
    {
        private readonly IDerivedFacade derivedFacade;

        public DerivedFromConcreateBaseEntity(int id, IDerivedFacade facade)
            : base(id, facade)
        {
            this.derivedFacade = facade;
        }

        public bool HasValidDerivedFacade()
        {
            return derivedFacade != null && derivedFacade.HasDomainEventPublisher();
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
