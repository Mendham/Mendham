using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham.Equality;

namespace Mendham.Domain
{
	public abstract class Entity : IEntity
	{
		IEnumerable<object> IHasEqualityComponents.EqualityComponents
		{
			get
			{
				return this.EqualityComponents;
			}
		}

		protected abstract IEnumerable<object> EqualityComponents { get; }

		public override bool Equals(object obj)
		{
			return this.EqualsFromComponents(obj);
		}

		public override int GetHashCode()
		{
			return this.GetHashCodeFromComponents();
		}
	}
}