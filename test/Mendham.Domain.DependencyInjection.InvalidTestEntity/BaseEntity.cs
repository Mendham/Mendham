using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.InvalidTestEntity
{
    public class BaseEntity : Entity
    {
        private readonly IBaseFacade domainFacade;

        public BaseEntity(int id, IBaseFacade facade)
        {
            this.Id = id;

            this.domainFacade = facade;
        }

        public int Id { get; private set; }

        public bool HasValidFacade()
        {
            return domainFacade != null && domainFacade.HasDomainEventPublisher();
        }

        public interface IBaseFacade : IDomainFacade
        {
            bool HasDomainEventPublisher();
        }

        public class BaseFacade : DomainFacade, IBaseFacade
        {
            private readonly bool domainEventPublisherHasValue;

            public BaseFacade(IDomainEventPublisher domainEventPublisher)
                : base(domainEventPublisher)
            {
                domainEventPublisherHasValue = domainEventPublisher != null;
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