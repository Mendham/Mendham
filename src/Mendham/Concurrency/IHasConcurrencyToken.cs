using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBeall.Mendham.Concurrency
{
	public interface IHasConcurrencyToken
	{
		ConcurrencyToken Token { get; }
		void Update(ConcurrencyToken token);
	}
}
