using Mendham.Domain.Events;
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
        private readonly IDomainEventPublisher domainEventPublisher;
        private readonly IEntityFactory entityFactory;

        public EntityCreationService(IEntityFactory entityFactory, IDomainEventPublisher domainEventPublisher)
        {
            this.entityFactory = entityFactory;
            this.domainEventPublisher = domainEventPublisher;
        }

        public async Task<Entity1> CreateEntityAsync()
        {
            var entity = entityFactory.Create();

            await domainEventPublisher.RaiseAsync(new EntityCreated());

            return entity;
        }
    }
}
