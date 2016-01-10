using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain
{
    public abstract class Entity<TEntity, TIdentity> : Entity<TEntity>, IEntity<TIdentity>
        where TEntity : Entity<TEntity>
        where TIdentity : IHasEqualityComponents
    {
        private readonly TIdentity _id;

        public Entity(TIdentity id)
        {
            _id = id
                .VerifyArgumentNotDefaultValue(nameof(id), "Entity ID is required");
        }

        public TIdentity Id { get { return _id; } }

        protected sealed override IEnumerable<object> IdentityComponents
        {
            get
            {
                yield return _id;
            }
        }

        protected internal sealed override Type GetIdentityComponentsDeclaringType()
        {
            return typeof(Entity<TEntity, TIdentity>);
        }
    }
}
