using Moq.Language.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq
{
    public static class ReturnExtensions
    {
        /// <summary>
        /// For a method that returns a enumeration, returns items (passed as parameters) as a collection of those items
        /// </summary>
        /// <typeparam name="T">Type of mock</typeparam>
        /// <typeparam name="TResult">Type of results returned by setup</typeparam>
        /// <param name="setup">Setup of mock</param>
        /// <param name="results">Additional items to be returned</param>
		public static IReturnsResult<T> ReturnsItems<T, TResult>(this ISetup<T, IEnumerable<TResult>> setup, params TResult[] results)
            where T : class
        {
            return setup.Returns(results);
        }

        /// <summary>
        /// For a method that returns a enumeration, returns items (passed as parameters) as a collection of those items
        /// </summary>
        /// <typeparam name="T">Type of mock</typeparam>
        /// <typeparam name="TResult">Type of results returned by setup</typeparam>
        /// <param name="setup">Setup of mock</param>
        /// <param name="results">Additional items to be returned</param>
		public static IReturnsResult<T> ReturnsItemsAsync<T, TResult>(this ISetup<T, Task<IEnumerable<TResult>>> setup, params TResult[] results)
            where T : class
        {
            return setup.Returns(Task.FromResult<IEnumerable<TResult>>(results));
        }

        /// <summary>
        /// Allows for a mock setup to return a different result on subsequent calls based as defined by the collection of results
        /// </summary>
        /// <typeparam name="T">Type of mock</typeparam>
        /// <typeparam name="TResult">Type of results returned by setup</typeparam>
        /// <param name="setup">Setup of mock</param>
        /// <param name="results">Set of items to be returned on subsequent calls</param>
		public static IReturnsResult<T> ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup, params TResult[] results)
            where T : class
        {
            return setup.Returns(new Queue<TResult>(results).Dequeue);
        }

        /// <summary>
        /// Allows for a mock setup to return a different result on subsequent calls based as defined by the collection of results
        /// </summary>
        /// <typeparam name="T">Type of mock</typeparam>
        /// <typeparam name="TResult">Type of results returned by setup</typeparam>
        /// <param name="setup">Setup of mock</param>
        /// <param name="results">Set of items to be returned on subsequent calls</param>
		public static IReturnsResult<TMock> ReturnsInOrderAsync<TMock, TResult>(this ISetup<TMock, Task<TResult>> setup, params TResult[] results)
            where TMock : class
        {
            var taskResults = results.Select(a =>
            {
                var tcs = new TaskCompletionSource<TResult>();
                tcs.SetResult(a);
                return tcs.Task;
            });

            return setup.Returns(new Queue<Task<TResult>>(taskResults).Dequeue);
        }

        /// <summary>
        /// Returns an empty task for a ISetup<TMock, Task>
        /// </summary>
        /// <typeparam name="T">Type of Mock</typeparam>
        /// <param name="setup">Setup of Mock that expects a return type of Task</param>
		public static IReturnsResult<T> ReturnsTask<T>(this ISetup<T, Task> setup)
            where T : class
        {
            return setup.Returns(Task.FromResult(0));
        }
    }
}
