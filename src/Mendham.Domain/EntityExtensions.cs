using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain
{
    public static class EntityExtensions
    {
        public static bool IsEqualToEntity(this IEntity entity, IEntity other)
        {
            if (entity == null && other == null)
                return true;

            if (entity == null || other == null)
                return false;

            if (ReferenceEquals(entity, other))
                return true;

            var hasEqualComponents = entity
                .AsEqualityComponentsObject()
                .AreComponentsEqual(other.AsEqualityComponentsObject());

            if (!hasEqualComponents)
                return false;

            // This needs to be updated to check level of identity
            return true;
        }

        public static int GetEntityHashCode(this IEntity entity)
        {
            // This needs to be updated to check for level identity is defined
            var seed = entity.GetType().GetHashCode();

            return entity
                .AsEqualityComponentsObject()
                .EqualityComponents
                .GetHashCodeForObjects(seed);
        }


        private static IHasEqualityComponents AsEqualityComponentsObject(this IEntity entity)
        {
            return new EntityComponents(entity.IdentityComponents);
        }

        private class EntityComponents : IHasEqualityComponents
        {
            private readonly IEnumerable<object> components;

            public EntityComponents(IEnumerable<object> components)
            {
                components.VerifyArgumentNotNullOrEmpty("Components for entity are not defined.");

                this.components = components;
            }

            public IEnumerable<object> EqualityComponents
            {
                get
                {
                    return components;
                }
            }
        }


    }
}
