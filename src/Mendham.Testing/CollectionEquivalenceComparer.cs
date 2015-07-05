using System;
using System.Collections.Generic;
using System.Linq;
using Mendham.Equality;

namespace Mendham.Testing
{
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
				.GetHashCodeForComponents();
		}

		public static CollectionEquivalenceComparer<T> Default
		{ 
			get
			{
				return new CollectionEquivalenceComparer<T>();
			}
		}

	}
}