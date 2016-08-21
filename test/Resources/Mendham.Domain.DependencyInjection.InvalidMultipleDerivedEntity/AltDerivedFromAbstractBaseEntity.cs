using Mendham.Events;

namespace Mendham.Domain.DependencyInjection.InvalidMultipleDerivedEntity
{
    public class AltDerivedFromAbstractBaseEntity : AbstractBaseEntity
    {
        private readonly IDerivedFacade derivedFacade;

        public AltDerivedFromAbstractBaseEntity(int id, IDerivedFacade facade)
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
