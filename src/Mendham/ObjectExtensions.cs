using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Converts T into an IEnumerable<T> that has the original object as its only item
		/// </summary>
		/// <typeparam name="T">Type of enumerable</typeparam>
		/// <param name="obj">Object to add to collection</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static IEnumerable<T> AsSingleItemEnumerable<T>(this T obj)
		{
            yield return obj;
		}

        /// <summary>
		/// Converts T into an List<T> that has the original object as its only item
		/// </summary>
		/// <typeparam name="T">Type of List</typeparam>
		/// <param name="obj">Object to add to collection</param>
		/// <returns></returns>
		[DebuggerStepThrough]
        public static List<T> AsSingleItemList<T>(this T obj)
        {
            return obj.AsSingleItemEnumerable()
                .ToList();
        }
    }
}
