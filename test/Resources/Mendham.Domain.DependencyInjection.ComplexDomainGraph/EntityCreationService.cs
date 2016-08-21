using Mendham.Domain.Events;
using Mendham.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.ComplexDomainGraph
{
    public interface IEntityCreationService
    {
        Task<Entity1> CreateEntityAsync();
    }

    public class EntityCreationService : IEntityCreationService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEntityFactory _entityFactory;

        public EntityCreationService(IEntityFactory entityFactory, IEventPublisher eventPublisher)
        {
            _entityFactory = entityFactory;
            _eventPublisher = eventPublisher;
        }

        public async Task<Entity1> CreateEntityAsync()
        {
            var entity = _entityFactory.Create();

            await _eventPublisher.RaiseAsync(new EntityCreated());

            return entity;
        }
    }
}
