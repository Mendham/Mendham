using Mendham.Domain.Events;
using Mendham.Events;
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
        private readonly IEventPublisher _eventPublisher;
        private readonly ICountService _countService;

        public HasCircularHandlerService(IEventPublisher eventPublisher, ICountService countService)
        {
            _eventPublisher = eventPublisher;
            _countService = countService;
        }

        public async Task<bool> StartAsync()
        {
            await _eventPublisher.RaiseAsync(new DomainEvent1());
            return _countService.WasMaxCountReached();
        }
    }
}
