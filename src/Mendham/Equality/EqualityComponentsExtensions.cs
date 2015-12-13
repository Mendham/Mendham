using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Equality
{
	public static class EqualityComponentsExtensions
	{
        /// <summary>
        /// Determines if two IHasEqualityComponents objects have equal components
        /// </summary>
        /// <param name="objectWithEqualityComponents"></param>
        /// <param name="otherObj"></param>
        /// <returns></returns>
        public static bool AreComponentsEqual(this IHasEqualityComponents objectWithEqualityComponents, object otherObj)
		{
            return objectWithEqualityComponents
                .AreComponentsEqual(otherObj as IHasEqualityComponents);
		}

        /// <summary>
		/// Determines if two IHasEqualityComponents objects have an equal components without regard for the object itself
		/// </summary>
		/// <param name="objectWithEqualityComponents"></param>
		/// <param name="otherObj"></param>
		/// <returns></returns>
		public static bool AreComponentsEqual<T>(this T objectWithEqualityComponents, T otherObj)
            where T : IHasEqualityComponents
        {
            if (objectWithEqualityComponents == null)
                throw new NullReferenceException("Object being checked by HaveEqualComponents cannot be null");

            if (otherObj == null)
                return false;

            if (ReferenceEquals(objectWithEqualityComponents, otherObj))
                return true;

            return objectWithEqualityComponents
                .EqualityComponents
                .SequenceEqual(otherObj.EqualityComponents, HasEqualityComponentsComparer.Default);
        }

        /// <summary>
        /// Gets a hash code for IHasEqualityComponents based on the objects and seeded by the type of parent object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetObjectWithEqualityComponentsHashCode(this IHasEqualityComponents obj)
        {
            var seed = obj.GetType().GetHashCode();
            return obj.EqualityComponents.GetHashCodeForObjects(seed);
        }

        /// <summary>
        /// Gets a hash code collection of objects passed
        /// </summary>
        /// <param name="objects">Objects to compare</param>
        /// <param name="seed">(Optional) Seed to start hash by</param>
        /// <returns></returns>
        public static int GetHashCodeForObjects(this IEnumerable<object> objects, int seed = 0)
        {
            var comparer = HasEqualityComponentsComparer.Default;

            return objects
                .Aggregate(seed, (prev, obj) => prev ^ comparer.GetHashCode(obj));
        }
    }
}
