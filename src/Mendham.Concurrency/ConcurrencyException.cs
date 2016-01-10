using System;

namespace Mendham.Concurrency
{
	public abstract class ConcurrencyException : Exception
	{
		public IHasConcurrencyToken Object { get; private set; }

		public ConcurrencyException(IHasConcurrencyToken obj, string message = null)
			: base(message)
		{
			this.Object = obj;
		}
	}
}
