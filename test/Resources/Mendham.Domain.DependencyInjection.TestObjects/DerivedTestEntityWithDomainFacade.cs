using Mendham.Domain.Events;

namespace Mendham.Domain.DependencyInjection.TestObjects
{
    public class DerivedTestEntityWithDomainFacade : AbstractTestEntityWithDomainFacade
    {
        private readonly IDerivedFacade derivedFacade;

        public DerivedTestEntityWithDomainFacade(int id, IDerivedFacade facade)
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
            private readonly bool domainEventPublisherHasValue;

            public DerivedFacade(IDomainEventPublisher domainEventPublisher)
                : base(domainEventPublisher)
            {
                domainEventPublisherHasValue = domainEventPublisher != null;
            }

            public bool HasValidDerivedFacade()
            {
                return domainEventPublisherHasValue;
            }
        }
    }
}
