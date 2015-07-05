using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Equality
{
	/// <summary>
	/// A contract that defines the properties of the object that are used to determine equality. It can be thought of as the objects "key"
	/// </summary>
	public interface IHasEqualityComponents
	{
		/// <summary>
		/// A set of objects that are used to determine equality between the implementing object and a second object
		/// </summary>
		IEnumerable<object> EqualityComponents { get; }
	}
}
