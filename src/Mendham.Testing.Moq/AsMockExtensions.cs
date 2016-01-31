using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;

namespace Mendham.Testing.Moq
{
	public static class AsMockExtensions
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
	}
}
