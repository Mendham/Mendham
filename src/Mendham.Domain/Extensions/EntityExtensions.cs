using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Domain.Extensions
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

            if (entity.GetType() == other.GetType())
                return true;

            if (!entity.IsOneEntityDerivedFromTheOther(other))
                return false;

            // When they aren't the same type but are derived, check to see if they use the same identity components
            return entity.GetIdentityComponentsDeclaringType() == other.GetIdentityComponentsDeclaringType();
        }

        public static int GetEntityHashCode(this IEntity entity)
        {
            // This needs to be updated to check for level identity is defined
            var seed = entity
                .GetIdentityComponentsDeclaringType()
                .GetHashCode();

            return entity
                .AsEqualityComponentsObject()
                .EqualityComponents
                .GetHashCodeForObjects(seed);
        }

        public static IEnumerable<object> GetIdentityComponents<T>(this IEntity<T> entity)
        {
            return entity.Id.AsSingleItemEnumerable<object>();
        }

        private static bool IsOneEntityDerivedFromTheOther(this IEntity entity, IEntity other)
        {
            return entity.GetType().IsAssignableFrom(other.GetType()) ||
                other.GetType().IsAssignableFrom(entity.GetType());
        }

        private static Type GetIdentityComponentsDeclaringType(this IEntity entity)
        {
            var identityComponentsDeclaringTypeCache = entity as IIdentityComponentsDeclaringTypeCache;

            if (identityComponentsDeclaringTypeCache != null)
            {
                return identityComponentsDeclaringTypeCache.GetIdentityComponentsDeclaringType();
            }
            else
            {
                return GetNonCachedIdentityComponentsDeclaringType(entity.GetType());
            }
        }

        private static Type GetNonCachedIdentityComponentsDeclaringType(Type entityType)
        {
            var explicitIdentityComponentsPropertyInfo = entityType
                .GetProperty("Mendham.Domain.IEntity.IdentityComponents", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            if (explicitIdentityComponentsPropertyInfo != null)
                return explicitIdentityComponentsPropertyInfo.DeclaringType;

            var implicitIdentityComponentsPropertyInfo = entityType
                .GetProperty("IdentityComponents", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            if (implicitIdentityComponentsPropertyInfo != null)
                return implicitIdentityComponentsPropertyInfo.DeclaringType;

            Type baseType = entityType.GetTypeInfo().BaseType;

            if (baseType == null)
                throw new InvalidOperationException("Could not find IdentityComponentsPropertyInfo");

            return GetNonCachedIdentityComponentsDeclaringType(baseType);
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
