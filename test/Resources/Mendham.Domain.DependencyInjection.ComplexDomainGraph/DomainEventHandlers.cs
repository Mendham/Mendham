using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.ComplexDomainGraph
{
    public class DomainEvent1Handler : IDomainEventHandler<DomainEvent1>
    {
        private readonly IDomainEventPublisher domainEventPublisher;
        private readonly IOtherService otherService;

        public DomainEvent1Handler(IDomainEventPublisher domainEventPublisher, IOtherService otherService)
        {
            this.domainEventPublisher = domainEventPublisher;
            this.otherService = otherService;
        }

        public Task HandleAsync(DomainEvent1 domainEvent)
        {
            var tasks = new List<Task>
            {
                domainEventPublisher.RaiseAsync(new DomainEvent2()),
                domainEventPublisher.RaiseAsync(new DomainEvent3())
            };

            return Task.WhenAll(tasks);
        }
    }

    public class DomainEvent2Handler : IDomainEventHandler<DomainEvent2>
    {
        private readonly IDomainEventPublisher domainEventPublisher;
        private readonly IOtherService otherService;

        public DomainEvent2Handler(IDomainEventPublisher domainEventPublisher, IOtherService otherService)
        {
            this.domainEventPublisher = domainEventPublisher;
            this.otherService = otherService;
        }

        public Task HandleAsync(DomainEvent2 domainEvent)
        {
            return domainEventPublisher.RaiseAsync(new DomainEvent3());
        }
    }

    public class DomainEvent3Handler : IDomainEventHandler<DomainEvent3>
    {
        private readonly IDomainEventPublisher domainEventPublisher;
        private readonly IOtherService otherService;

        public DomainEvent3Handler(IDomainEventPublisher domainEventPublisher, IOtherService otherService)
        {
            this.domainEventPublisher = domainEventPublisher;
            this.otherService = otherService;
        }

        public Task HandleAsync(DomainEvent3 domainEvent)
        {
            var tasks = new List<Task>
            {
                domainEventPublisher.RaiseAsync(new DomainEvent4()),
                domainEventPublisher.RaiseAsync(new DomainEvent5())
            };

            return Task.WhenAll(tasks);
        }
    }

    public class DomainEvent4Handler : IDomainEventHandler<DomainEvent4>
    {
        private readonly IDomainEventPublisher domainEventPublisher;
        private readonly IEntityCreationService entityCreationService;

        public DomainEvent4Handler(IDomainEventPublisher domainEventPublisher, IEntityCreationService entityCreationService)
        {
            this.domainEventPublisher = domainEventPublisher;
            this.entityCreationService = entityCreationService;
        }

        public async Task HandleAsync(DomainEvent4 domainEvent)
        {
            var entity = await entityCreationService.CreateEntityAsync();

            var entityActionResult = await entity.InvokeEntityActionAsync();

            if (!entityActionResult)
            {
                await domainEventPublisher.RaiseAsync(new DomainEvent5());
            }
        }
    }

    public class DomainEvent5Handler : IDomainEventHandler<DomainEvent5>
    {
        private readonly IDomainEventPublisher domainEventPublisher;
        private readonly ICountService countService;
        private readonly IEntityCreationService entityCreationService;
        private readonly IEntityFactory entityFactory;

        public DomainEvent5Handler(IDomainEventPublisher domainEventPublisher, ICountService countService,
             IEntityCreationService entityCreationService, IEntityFactory entityFactory)
        {
            this.domainEventPublisher = domainEventPublisher;
            this.countService = countService;
            this.entityCreationService = entityCreationService;
            this.entityFactory = entityFactory;
        }

        public async Task HandleAsync(DomainEvent5 domainEvent)
        {
            bool continueTest = countService.ShouldContinue();

            if (continueTest)
            {
                await domainEventPublisher.RaiseAsync(new DomainEvent1());
            }
        }
    }
}
