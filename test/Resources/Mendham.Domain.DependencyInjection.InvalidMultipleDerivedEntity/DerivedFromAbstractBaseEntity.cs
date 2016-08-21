using Mendham.Domain.Events;
using Mendham.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.InvalidMultipleDerivedEntity
{
    public class DerivedFromAbstractBaseEntity : AbstractBaseEntity
    {
        private readonly IDerivedFacade derivedFacade;

        public DerivedFromAbstractBaseEntity(int id, IDerivedFacade facade)
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
