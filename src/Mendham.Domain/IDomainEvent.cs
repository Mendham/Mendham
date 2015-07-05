using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain
{
	public interface IDomainEvent
	{
		DateTime TimeOccurred { get; }
	}
}
