using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;

namespace Mendham.Testing
{
	public static class MoqExtensions
	{
		public static Mock<T> AsMock<T>(this T obj)
			where T : class
		{
			return Mock.Get(obj);
		}

		public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup, params TResult[] results)
			where T : class
		{
			setup.Returns(new Queue<TResult>(results).Dequeue);
		}

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

		public static void ReturnsNoActionTask<TMock>(this ISetup<TMock, Task> setup)
			where TMock : class
		{
			setup.Returns(Task.FromResult(0));
		}
	}
}
