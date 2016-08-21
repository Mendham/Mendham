using Mendham.Domain.Events;
using System.Threading.Tasks;

namespace Mendham.Domain
{
	public abstract class DomainFacade : IDomainFacade
	{
		private readonly IDomainEventPublisher _domainEventPublisher;

		public DomainFacade(IDomainEventPublisher domainEventPublisher)
		{
			_domainEventPublisher = domainEventPublisher;
		}

		public Task RaiseEventAsync<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : class, IDomainEvent
		{
			return _domainEventPublisher.RaiseAsync(domainEvent);
		}
	}
}