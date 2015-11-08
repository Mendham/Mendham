﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;

namespace Mendham.Testing.Moq
{
	public static class MoqExtensions
	{
        /// <summary>
        /// Converts an object of Mock<T> to a Mock<T>"/>
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="obj">Object of Mock<T></param>
        /// <returns>Mock of the object</returns>
		public static Mock<T> AsMock<T>(this T obj)
			where T : class
		{
			return Mock.Get(obj);
		}

        /// <summary>
        /// Allows for a mock setup to return a different result on subsequent calls based as defined by the collection of results
        /// </summary>
        /// <typeparam name="T">Type of mock</typeparam>
        /// <typeparam name="TResult">Type of results returned by setup</typeparam>
        /// <param name="setup">Setup of mock</param>
        /// <param name="results">Set of items to be returned on subsequent calls</param>
		public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup, params TResult[] results)
			where T : class
		{
			setup.Returns(new Queue<TResult>(results).Dequeue);
		}

        /// <summary>
        /// Allows for a mock setup to return a different result on subsequent calls based as defined by the collection of results
        /// </summary>
        /// <typeparam name="T">Type of mock</typeparam>
        /// <typeparam name="TResult">Type of results returned by setup</typeparam>
        /// <param name="setup">Setup of mock</param>
        /// <param name="results">Set of items to be returned on subsequent calls</param>
		public static void ReturnsInOrderAsync<TMock, TResult>(this ISetup<TMock, Task<TResult>> setup, params TResult[] results)
			where TMock : class
		{
			var taskResults = results.Select(a =>
			{
				var tcs = new TaskCompletionSource<TResult>();
				tcs.SetResult(a);
				return tcs.Task;
			});

			setup.Returns(new Queue<Task<TResult>>(taskResults).Dequeue);
		}

        /// <summary>
        /// Returns an empty task for a ISetup<TMock, Task>
        /// </summary>
        /// <typeparam name="TMock">Type of Mock</typeparam>
        /// <param name="setup">Setup of Mock that expects a return type of Task</param>
		public static void ReturnsNoActionTask<TMock>(this ISetup<TMock, Task> setup)
			where TMock : class
		{
			setup.Returns(Task.FromResult(0));
		}
	}
}
