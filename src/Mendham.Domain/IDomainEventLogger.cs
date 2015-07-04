using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBeall.Mendham.Domain
{
	public interface IDomainEventLogger
	{
		void LogDomainEvent<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent;
	}
}
