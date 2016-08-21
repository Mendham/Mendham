using Mendham.Events;
using System.Collections.Generic;

namespace Mendham.Domain.DependencyInjection.InvalidConcreateBaseEntity
{
    public class ConcreateBaseEntity : Entity
    {
        private readonly IBaseFacade _domainFacade;

        public ConcreateBaseEntity(int id, IBaseFacade facade)
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

        public class BaseFacade : DomainFacade, IBaseFacade
        {
            private readonly bool domainEventPublisherHasValue;

            public BaseFacade(IEventPublisher eventPublisher)
                : base(eventPublisher)
            {
                domainEventPublisherHasValue = eventPublisher != null;
            }

            public bool HasDomainEventPublisher()
            {
                return domainEventPublisherHasValue;
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