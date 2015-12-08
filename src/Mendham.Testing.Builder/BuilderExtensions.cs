using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// Builds multiple objects using builder based on the criteria passed. This can be used to do things
        /// such as build multiple children that have the same parent
        /// </summary>
        /// <typeparam name="T">Type being built</typeparam>
        /// <param name="builder">Builder</param>
        /// <param name="count">Items to be built</param>
        /// <returns>A list of T items built.</returns>
        public static List<T> BuildMultiple<T>(this IBuilder<T> builder, int count)
        {
            count.VerifyArgumentMeetsCriteria(a => a >= 1, "Count to build multiple must be at least one");

            return Enumerable.Range(0, count)
                .Select(a => builder.Build())
                .ToList();
        }
    }
}
