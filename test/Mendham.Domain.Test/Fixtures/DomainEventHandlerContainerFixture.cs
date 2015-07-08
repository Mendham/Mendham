using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham.Domain.Events;
using Mendham.Testing;

namespace Mendham.Domain.Test.Fixtures
{
    public class DomainEventHandlerContainerFixture : BaseFixture<DomainEventHandlerContainer>
    {
		public IEnumerable<IDomainEventHandler> DomainEventHandlers { get; set; }

		public override DomainEventHandlerContainer CreateSut()
		{
			return new DomainEventHandlerContainer(this.DomainEventHandlers);
		}

		public override void ResetFixture()
		{
			base.ResetFixture();

			DomainEventHandlers = Enumerable.Empty<IDomainEventHandler>();
		}
	}
}
