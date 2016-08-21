using Mendham.Domain.Events;
using Mendham.Events;
using System.Collections.Generic;

namespace Mendham.Domain.DependencyInjection.TestObjects
{
    public abstract class AbstractTestEntityWithDomainFacade : Entity
    {
        private readonly IBaseFacade _domainFacade;

        public AbstractTestEntityWithDomainFacade(int id, IBaseFacade facade)
        {
            Id = id;
            _domainFacade = facade;
        }

        public int Id { get; }

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
