using Mendham.Events;
using System.Collections.Generic;

namespace Mendham.Domain.DependencyInjection.TestObjects
{
    public class TestEntityWithDomainFacade : Entity
    {
        public delegate TestEntityWithDomainFacade Factory(int id);

        private readonly IFacade _domainFacade;

        public TestEntityWithDomainFacade(int id, IFacade facade)
        {
            Id = id;
            _domainFacade = facade;
        }

        public int Id { get; private set; }

        public bool HasValidFacade()
        {
            return _domainFacade != null && _domainFacade.HasDomainEventPublisher();
        }

        public interface IFacade : IDomainFacade
        {
            bool HasDomainEventPublisher();
        }

        public class Facade : DomainFacade, IFacade, IUnrelatedInterface
        {
            private readonly bool _eventPublisherHasValue;

            public Facade(IEventPublisher eventPublisher)
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
