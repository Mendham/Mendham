using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham.Domain.Events;

namespace Mendham.Domain.DependencyInjection.Autofac.Test.Data
{
	public sealed class Test1DomainEventHandler : DomainEventHandler<Test1DomainEvent>
	{
		public override Task HandleAsync(Test1DomainEvent domainEvent)
		{
			return Task.FromResult(0);
		}
	}

	public sealed class Test2DomainEventHandler : DomainEventHandler<Test2DomainEvent>
	{
		public override Task HandleAsync(Test2DomainEvent domainEvent)
		{
			return Task.FromResult(0);
		}
	}

	public sealed class Test1DomainEvent : DomainEvent
	{ }

	public sealed class Test2DomainEvent : DomainEvent
	{ }
}
