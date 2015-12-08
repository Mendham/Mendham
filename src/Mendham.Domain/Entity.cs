using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham.Equality;

namespace Mendham.Domain
{
    /// <summary>
    /// Base class for entity that does not implement IEquatable. To have IEquatable already implemented, use Entity<T>.
    /// </summary>
	public abstract class Entity : IEntity
	{
		IEnumerable<object> IHasEqualityComponents.EqualityComponents
		{
			get
			{
				return this.IdentityComponents;
			}
		}

        /// <summary>
        /// The property(s) that make up the identity of the entity used for equality
        /// </summary>
		protected abstract IEnumerable<object> IdentityComponents { get; }

		public override bool Equals(object obj)
		{
			return this.HaveEqualComponents(obj) && this.IsObjectSameType(obj);
		}

		public override int GetHashCode()
		{
			return this.GetHashCodeForObjectWithComponents();
		}

        public static bool operator ==(Entity a, Entity b)
        {
            return object.Equals(a, b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }
    }

    /// <summary>
    /// Base class for entity that also implements IEquatable for the type
    /// </summary>
    /// <typeparam name="T">The derived type of Entity<T></typeparam>
    public abstract class Entity<T> : Entity, IEquatable<T>
        where T : Entity<T>, IHasEqualityComponents
    {
        public bool Equals(T other)
        {
            return ((T)this).HaveEqualComponents<T>(other) && this.IsObjectSameType(other);
        }

        public static explicit operator T (Entity<T> entity)
        {
            return entity as T;
        }
    }
}