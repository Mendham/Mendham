using Mendham.Domain.Events;
using Mendham.Events;
using System.Threading.Tasks;

namespace Mendham.Domain
{
	public abstract class DomainFacade : IDomainFacade
	{
		private readonly IEventPublisher _eventPublisher;

		public DomainFacade(IEventPublisher eventPublisher)
		{
			_eventPublisher = eventPublisher;
		}

		public Task RaiseEventAsync<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : class, IDomainEvent
		{
			return _eventPublisher.RaiseAsync(domainEvent);
		}
	}
}