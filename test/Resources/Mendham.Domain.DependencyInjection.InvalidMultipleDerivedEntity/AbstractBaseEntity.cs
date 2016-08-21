using Mendham.Domain.Events;
using Mendham.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.InvalidMultipleDerivedEntity
{
    public abstract class AbstractBaseEntity : Entity
    {
        private readonly IBaseFacade _domainFacade;

        public AbstractBaseEntity(int id, IBaseFacade facade)
        {
            Id = id;
            _domainFacade = facade;
        }

        public int Id { get; private set; }

        public bool HasValidFacade()
        {
            return _domainFacade != null && _domainFacade.HasDomainEventPublisher();
        }

        public interface IBaseFacade : IDomainFacade
        {
            bool HasDomainEventPublisher();
        }

        public abstract class BaseFacade : DomainFacade, IBaseFacade
        {
            private readonly bool _eventPublisherHasValue;

            public BaseFacade(IEventPublisher eventPublisher)
                : base(eventPublisher)
            {
                _eventPublisherHasValue = eventPublisher != null;
            }

            public bool HasDomainEventPublisher()
            {
                return _eventPublisherHasValue;
            }
        }

        protected override IEnumerable<object> IdentityComponents
        {
            get
            {
                yield return Id;
            }
        }
    }
}