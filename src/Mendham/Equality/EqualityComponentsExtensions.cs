using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Equality
{
	public static class EqualityComponentsExtensions
	{
		/// <summary>
		/// Gets a hash code for a set of components
		/// </summary>
		/// <param name="objectWithEqualityComponents"></param>
		/// <returns></returns>
		public static int GetHashCodeFromComponents(this IHasEqualityComponents objectWithEqualityComponents)
		{
			return objectWithEqualityComponents.EqualityComponents.GetHashCodeForComponents();
		}

		/// <summary>
		/// Determines if two IHasEqualityComponents objects are equal based on their components
		/// </summary>
		/// <param name="objectWithEqualityComponents"></param>
		/// <param name="otherObj"></param>
		/// <returns></returns>
		public static bool EqualsFromComponents(this IHasEqualityComponents objectWithEqualityComponents, object otherObj)
		{
			if (object.ReferenceEquals(objectWithEqualityComponents, otherObj))
				return true;

			var otherObjectAsHasEqualityComponents = otherObj as IHasEqualityComponents;

			if (otherObjectAsHasEqualityComponents == null || objectWithEqualityComponents.GetType() != otherObj.GetType())
				return false;

			return objectWithEqualityComponents.EqualityComponents.SequenceEqual(otherObjectAsHasEqualityComponents.EqualityComponents);
		}

		/// <summary>
		/// Gets a hash code for a set of components. The order in which the objects are passed does matter.
		/// </summary>
		/// <param name="objects"></param>
		/// <returns></returns>
		public static int GetHashCodeForComponents(this IEnumerable<object> objects)
		{
			unchecked
			{
				return new int[] { 19 }
					.Union(objects
						.Where(a => a != null)
						.Select(a => a.GetHashCode()))
					.Aggregate((sum, next) => sum * 13 + next);
			}
		}
	}
}
