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
				return this.IdentityComponents;
			}
		}

		protected abstract IEnumerable<object> IdentityComponents { get; }

		public override bool Equals(object obj)
		{
			return this.HaveEqualComponents(obj);
		}

		public override int GetHashCode()
		{
			return this.GetHashCodeForObjectWithComponents();
		}
	}
}