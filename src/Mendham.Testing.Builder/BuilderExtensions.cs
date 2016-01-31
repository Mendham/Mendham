using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// Builds a collection of the object built by the builder factory
        /// </summary>
        /// <typeparam name="T">Object to be built</typeparam>
        /// <param name="builderFactory">Delegate to create builder</param>
        /// <param name="count">Number of items to build</param>
        /// <returns>An enumerable of the items</returns>
        public static IEnumerable<T> BuildMultiple<T>(this Func<IBuilder<T>> builderFactory, int count)
        {
            count.VerifyArgumentMeetsCriteria(a => a >= 1, nameof(count),
                "Count to build multiple must be at least one");

            return Enumerable.Range(0, count)
                .Select(a => builderFactory().Build())
                .ToArray();
        }
    }
}
