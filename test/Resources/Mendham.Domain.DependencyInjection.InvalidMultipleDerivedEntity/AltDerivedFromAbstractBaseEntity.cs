using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
