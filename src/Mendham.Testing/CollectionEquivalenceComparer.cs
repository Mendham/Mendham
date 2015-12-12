using System;
using System.Collections.Generic;
using System.Linq;
using Mendham.Equality;

namespace Mendham.Testing
{
    /// <summary>
    /// Equality Comparer used to determine if two collections contain the same elements regardless of order
    /// </summary>
    /// <typeparam name="T">Type within collection to be evaluated</typeparam>
	public class CollectionEquivalenceComparer<T> : IEqualityComparer<IEnumerable<T>>
		where T : IEquatable<T>
	{
		public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
		{
			List<T> leftList = new List<T>(x);
			List<T> rightList = new List<T>(y);
			leftList.Sort();
			rightList.Sort();

			IEnumerator<T> enumeratorX = leftList.GetEnumerator();
			IEnumerator<T> enumeratorY = rightList.GetEnumerator();

			while (true)
			{
				bool hasNextX = enumeratorX.MoveNext();
				bool hasNextY = enumeratorY.MoveNext();

				if (!hasNextX || !hasNextY)
					return (hasNextX == hasNextY);

				if (!enumeratorX.Current.Equals(enumeratorY.Current))
					return false;
			}
		}

		public int GetHashCode(IEnumerable<T> obj)
		{
			return obj
				.Cast<object>()
				.GetHashCodeForObjects();
		}

        /// <summary>
        /// Returns a CollectionEquivalenceComparer for use in comparing sets
        /// </summary>
		public static CollectionEquivalenceComparer<T> Default
		{ 
			get
			{
				return new CollectionEquivalenceComparer<T>();
			}
		}
	}
}