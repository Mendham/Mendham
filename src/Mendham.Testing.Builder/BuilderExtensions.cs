using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public static class BuilderExtensions
    {

        private static readonly Random _random = new Random();

        /// <summary>
        /// Uses <paramref name="builderFactory"/> to build a random amount objects of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Object to be built</typeparam>
        /// <param name="builderFactory">Delegate to create builder</param>
        /// <returns>An enumerable containing <typeparamref name="T"/> objects</returns>
        public static IEnumerable<T> BuildMultiple<T>(this Func<IBuilder<T>> builderFactory)
        {
            var count = _random.Next(4, 9);
            return builderFactory.BuildMultiple(count);
        }

        /// <summary>
        /// Uses <paramref name="builderFactory"/> to build multiple objects of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Object to be built</typeparam>
        /// <param name="builderFactory">Delegate to create builder</param>
        /// <param name="count">Number of items to build</param>
        /// <returns>An enumerable containing <typeparamref name="T"/> objects</returns>
        public static IEnumerable<T> BuildMultiple<T>(this Func<IBuilder<T>> builderFactory, int count)
        {
            count.VerifyArgumentMeetsCriteria(a => a >= 1, nameof(count),
                "Count to build multiple must be at least one");

            return Enumerable.Range(0, count)
                // For larger counts, parallel is more performant
                .AsParallel() 
                .Select(a => builderFactory().Build())
                .ToArray();
        }
    }
}
