using Mendham.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.ComplexDomainGraph
{
    public class DomainEvent1Handler : IEventHandler<DomainEvent1>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IOtherService _otherService;

        public DomainEvent1Handler(IEventPublisher eventPublisher, IOtherService otherService)
        {
            _eventPublisher = eventPublisher;
            _otherService = otherService;
        }

        public Task HandleAsync(DomainEvent1 domainEvent)
        {
            var tasks = new List<Task>
            {
                _eventPublisher.RaiseAsync(new DomainEvent2()),
                _eventPublisher.RaiseAsync(new DomainEvent3())
            };

            return Task.WhenAll(tasks);
        }
    }

    public class DomainEvent2Handler : IEventHandler<DomainEvent2>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IOtherService _otherService;

        public DomainEvent2Handler(IEventPublisher eventPublisher, IOtherService otherService)
        {
            _eventPublisher = eventPublisher;
            _otherService = otherService;
        }

        public Task HandleAsync(DomainEvent2 domainEvent)
        {
            return _eventPublisher.RaiseAsync(new DomainEvent3());
        }
    }

    public class DomainEvent3Handler : IEventHandler<DomainEvent3>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IOtherService _otherService;

        public DomainEvent3Handler(IEventPublisher eventPublisher, IOtherService otherService)
        {
            _eventPublisher = eventPublisher;
            _otherService = otherService;
        }

        public Task HandleAsync(DomainEvent3 domainEvent)
        {
            var tasks = new List<Task>
            {
                _eventPublisher.RaiseAsync(new DomainEvent4()),
                _eventPublisher.RaiseAsync(new DomainEvent5())
            };

            return Task.WhenAll(tasks);
        }
    }

    public class DomainEvent4Handler : IEventHandler<DomainEvent4>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEntityCreationService _entityCreationService;

        public DomainEvent4Handler(IEventPublisher eventPublisher, IEntityCreationService entityCreationService)
        {
            _eventPublisher = eventPublisher;
            _entityCreationService = entityCreationService;
        }

        public async Task HandleAsync(DomainEvent4 domainEvent)
        {
            var entity = await _entityCreationService.CreateEntityAsync();

            var entityActionResult = await entity.InvokeEntityActionAsync();

            if (!entityActionResult)
            {
                await _eventPublisher.RaiseAsync(new DomainEvent5());
            }
        }
    }

    public class DomainEvent5Handler : IEventHandler<DomainEvent5>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ICountService _countService;
        private readonly IEntityCreationService _entityCreationService;
        private readonly IEntityFactory _entityFactory;

        public DomainEvent5Handler(IEventPublisher eventPublisher, ICountService countService,
             IEntityCreationService entityCreationService, IEntityFactory entityFactory)
        {
            _eventPublisher = eventPublisher;
            _countService = countService;
            _entityCreationService = entityCreationService;
            _entityFactory = entityFactory;
        }

        public async Task HandleAsync(DomainEvent5 domainEvent)
        {
            bool continueTest = _countService.ShouldContinue();

            if (continueTest)
            {
                await _eventPublisher.RaiseAsync(new DomainEvent1());
            }
        }
    }
}
