using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mendham.Events;

namespace Mendham.Domain.DependencyInjection.ComplexDomainGraph
{
    public class Entity1 : Entity
    {
        private readonly IFacade _facade;

        public Entity1(IFacade facade)
        {
            _facade = facade;
            Id = Guid.NewGuid();
        }

        public async Task<bool> InvokeEntityActionAsync()
        {
            if (_facade.ShouldContinue())
            {
                await _facade.RaiseEventAsync(new DomainEvent5());
                return true;
            }

            return false;
        }

        public Guid Id { get; }

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
            private readonly ICountService _countService;

            public Facade(IEventPublisher eventPublisher, ICountService countService)
                : base(eventPublisher)
            {
                _countService = countService;
            }

            public bool ShouldContinue()
            {
                return _countService.ShouldContinue();
            }
        }
    }
}
