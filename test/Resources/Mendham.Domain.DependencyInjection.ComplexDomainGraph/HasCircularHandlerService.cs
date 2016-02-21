using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.ComplexDomainGraph
{
    public interface IHasCircularHandlerService
    {
        Task<bool> StartAsync();
    }

    public class HasCircularHandlerService : IHasCircularHandlerService
    {
        private readonly IDomainEventPublisher domainEventPublisher;
        private readonly ICountService countService;

        public HasCircularHandlerService(IDomainEventPublisher domainEventPublisher, ICountService countService)
        {
            this.domainEventPublisher = domainEventPublisher;
            this.countService = countService;
        }

        public async Task<bool> StartAsync()
        {
            await domainEventPublisher.RaiseAsync(new DomainEvent1());
            return countService.WasMaxCountReached();
        }
    }
}
