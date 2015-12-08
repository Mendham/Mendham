using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Equality
{
	public static class EqualityComponentsExtensions
	{
        /// <summary>
        /// Determines if two IHasEqualityComponents objects have an equal components without regard for the object itself
        /// </summary>
        /// <param name="objectWithEqualityComponents"></param>
        /// <param name="otherObj"></param>
        /// <returns></returns>
        public static bool HaveEqualComponents(this IHasEqualityComponents objectWithEqualityComponents, object otherObj)
		{
            if (objectWithEqualityComponents == null)
                throw new NullReferenceException("Object being checked by HaveEqualComponents cannot be null");

            if (ReferenceEquals(objectWithEqualityComponents, otherObj))
				return true;

			var otherObjectAsHasEqualityComponents = otherObj as IHasEqualityComponents;

			if (otherObjectAsHasEqualityComponents == null)
				return false;

			return objectWithEqualityComponents.EqualityComponents.SequenceEqual(otherObjectAsHasEqualityComponents.EqualityComponents);
		}

        /// <summary>
		/// Determines if two IHasEqualityComponents objects have an equal components without regard for the object itself
		/// </summary>
		/// <param name="objectWithEqualityComponents"></param>
		/// <param name="otherObj"></param>
		/// <returns></returns>
		public static bool HaveEqualComponents<T>(this T objectWithEqualityComponents, T otherObj)
            where T : IHasEqualityComponents
        {
            if (objectWithEqualityComponents == null)
                throw new NullReferenceException("Object being checked by HaveEqualComponents cannot be null");

            if (otherObj == null)
                return false;

            if (ReferenceEquals(objectWithEqualityComponents, otherObj))
                return true;

            return objectWithEqualityComponents.EqualityComponents.SequenceEqual(otherObj.EqualityComponents);
        }

        /// <summary>
        /// Deterines if an object is the same as the object implementing IHasEqualityComponents
        /// </summary>
        public static bool IsObjectSameType(this IHasEqualityComponents obj, object otherObject)
        {
            if (obj == null)
                throw new NullReferenceException("Object being checked by HaveEqualComponents cannot be null");

            if (otherObject == null)
                return false;

            if (ReferenceEquals(obj, otherObject))
                return true;

            return obj.GetType() == otherObject.GetType();
        }

        /// <summary>
        /// Gets a hash code for a set of components. The order in which the objects are passed does matter.
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static int GetHashCodeForComponents(this IEnumerable<object> objects)
		{
            return GetHashCodeForComponents(objects, 19);
		}

        /// <summary>
		/// Gets a hash code for object that implements IHasEqualityComponents.
		/// </summary>
        /// <param name="objectWithComponents">Object that contains the components</param>
		/// <returns></returns>
		public static int GetHashCodeForObjectWithComponents(this IHasEqualityComponents objectWithComponents)
        {
            var hashCodeOfObjectName = objectWithComponents.GetType().FullName.GetHashCode();
            return GetHashCodeForComponents(objectWithComponents.EqualityComponents, hashCodeOfObjectName);
        }

        private static int GetHashCodeForComponents(IEnumerable<object> objects, int startingValue)
        {
            unchecked
            {
                return new int[] { startingValue }
                    .Union(objects
                        .Select(a => a != null ? a.GetHashCode() : -7))
                    .Aggregate((sum, next) => sum * 13 + next);
            }
        }
    }
}
