using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham.Domain.Events;

namespace Mendham.Domain.DependencyInjection.ComplexDomainGraph
{
    public class Entity1 : Entity
    {
        private readonly IFacade facade;

        public Entity1(IFacade facade)
        {
            this.facade = facade;
            this.Id = Guid.NewGuid();
        }

        public async Task<bool> InvokeEntityActionAsync()
        {
            if (facade.ShouldContinue())
            {
                await facade.RaiseEventAsync(new DomainEvent5());
                return true;
            }

            return false;
        }

        public Guid Id { get; private set; }

        protected override IEnumerable<object> IdentityComponents
        {
            get
            {
                yield return Id;
            }
        }

        public interface IFacade : IDomainFacade
        {
            bool ShouldContinue();
        }

        public class Facade : DomainFacade, IFacade
        {
            private readonly ICountService countService;

            public Facade(IDomainEventPublisher domainEventPublisher, ICountService countService) 
                : base(domainEventPublisher)
            {
                this.countService = countService;
            }

            public bool ShouldContinue()
            {
                return countService.ShouldContinue();
            }
        }
    }
}
