using Mendham.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Domain
{
    /// <summary>
    /// Base class for entity that does not implement IEquatable. To have IEquatable already implemented, use Entity<T>.
    /// </summary>
	public abstract class Entity : IEntity, IIdentityComponentsDeclaringTypeCache
	{
		IEnumerable<object> IEntity.IdentityComponents
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
            return this.IsEqualToEntity(obj as IEntity);
        }

        public override int GetHashCode()
		{
            return this.GetEntityHashCode();
		}

        public static bool operator ==(Entity a, Entity b)
        {
            return a.IsEqualToEntity(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        private Type _identityComponentsDeclaringType = null;

        Type IIdentityComponentsDeclaringTypeCache.GetIdentityComponentsDeclaringType()
        {
            return GetIdentityComponentsDeclaringType();
        }

        internal protected virtual Type GetIdentityComponentsDeclaringType()
        {
            if (_identityComponentsDeclaringType == null)
            {
                _identityComponentsDeclaringType = GetType()
                    .GetProperty("IdentityComponents", BindingFlags.NonPublic | BindingFlags.Instance)
                    .DeclaringType;
            }

            return _identityComponentsDeclaringType;
        }
    }

    /// <summary>
    /// Base class for entity that also implements IEquatable for the type
    /// </summary>
    /// <typeparam name="T">The derived type of Entity<T></typeparam>
    public abstract class Entity<T> : Entity, IEquatable<T>
        where T : Entity<T>
    {
        public bool Equals(T other)
        {
            return this.IsEqualToEntity(other);
        }

        public static explicit operator T (Entity<T> entity)
        {
            return entity as T;
        }
    }
}