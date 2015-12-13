using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Domain.Extensions
{
    internal static class IdentityComponentsDeclaringTypeExtensions
    {
        internal static Type GetIdentityComponentsDeclaringType(this IEntity entity)
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

        internal static Type GetNonCachedIdentityComponentsDeclaringType(Type entityType)
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
    }
}
