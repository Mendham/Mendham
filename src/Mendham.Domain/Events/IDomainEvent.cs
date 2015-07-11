using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
	/// <summary>
	/// A domain event
	/// </summary>
	public interface IDomainEvent
	{
		/// <summary>
		/// Time in which the domain event occured
		/// </summary>
		DateTime TimeOccurred { get; }
	}
}
