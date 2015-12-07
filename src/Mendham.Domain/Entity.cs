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
	}

    /// <summary>
    /// Base class for entity that also implements IEquatable for the type
    /// </summary>
    /// <typeparam name="T">The derived type of Entity<T></typeparam>
    public abstract class Entity<T> : Entity, IEntity<T>, IEquatable<T>
        where T : Entity<T>, IEntity<T>, IHasEqualityComponents
    {
        public bool Equals(T other)
        {
            return ((T)this).HaveEqualComponents<T>(other) && this.IsObjectSameType(other);
        }

        public static explicit operator T (Entity<T> entity)
        {
            T entityObject = entity as T;

            if (entityObject == default(T))
            {
                var msg = "Entity {0} is not configured correctly. Its base class type must be T of itself, but is actually T of {1}";

                throw new InvalidOperationException(string.Format(msg, entityObject.GetType().FullName, typeof(T).FullName));
            }

            return entityObject;
        }
    }
}