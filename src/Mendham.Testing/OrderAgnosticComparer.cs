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
	public class OrderAgnosticComparer<T> : IEqualityComparer<IEnumerable<T>>
		where T : IEquatable<T>
	{
		public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
		{
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x.Count() != y.Count())
            {
                return false;
            }
            else if (x.SequenceEqual(y))
            {
                return true;
            }
            else
            {
                var yHash = y.Select(a => a.GetHashCode())
                    .OrderBy(a => a);

                return x.Select(a => a.GetHashCode())
                    .OrderBy(a => a)
                    .SequenceEqual(yHash);
            }
		}

		public int GetHashCode(IEnumerable<T> obj)
		{
            return obj
                .Select(a => a.GetHashCode())
                .OrderByDescending(a => a)
                .Select(GetIndexBasedHash)
                .Aggregate(GetLengthHashCode(obj), (prev, next) => prev ^ next);
		}

        private static int GetIndexBasedHash(int val, int index)
        {
            unchecked
            {
                return ((index + 739) * val) ^ index;
            }
        }

        private static int GetLengthHashCode(IEnumerable<T> obj)
        {
            unchecked
            {
                return obj.Count() * 997;
            }
        }

        /// <summary>
        /// Returns a CollectionEquivalenceComparer for use in comparing sets
        /// </summary>
		public static OrderAgnosticComparer<T> Default
		{ 
			get
			{
				return new OrderAgnosticComparer<T>();
			}
		}
	}
}