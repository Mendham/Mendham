using FluentAssertions;
using Mendham.Equality;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Test.Equality
{
	public class EqualityComponentsExtensionTests
	{
		public class TestObject : IHasEqualityComponents
		{
			public TestObject(string strVal, int intVal, object objVal)
			{
				this.StrVal = strVal;
				this.IntVal = intVal;
				this.ObjVal = objVal;
			}

			public string StrVal { get; set; }
			public int IntVal { get; set; }
			public object ObjVal { get; set; }


			public IEnumerable<object> EqualityComponents
			{
				get
				{
					yield return StrVal;
					yield return IntVal;
					yield return ObjVal;
				}
			}
		}

		[Theory]
		[MendhamData]
		public void AreComponentsEqual_SameObjectReference_Equal(TestObject obj)
		{
			var objRefCopy = obj;

			var equal = obj.AreComponentsEqual(objRefCopy);

			equal.Should().BeTrue();
		}

		[Theory]
		[MendhamData]
		public void AreComponentsEqual_DifferentObject_NotEqual(TestObject obj1, TestObject obj2)
		{
			var equal = obj1.AreComponentsEqual(obj2);

			equal.Should().BeFalse();
		}

		[Theory]
		[MendhamData]
		public void AreComponentsEqual_DifferentObjectWithSameComponents_Equal(string strVal, int intVal, object objVal)
		{
			var testObj1 = new TestObject(strVal, intVal, objVal);
			var testObj2 = new TestObject(strVal, intVal, objVal);

			var equal = testObj1.AreComponentsEqual(testObj2);

			testObj1.Should().NotBeSameAs(testObj2);
			equal.Should().BeTrue();
		}

		[Theory]
		[MendhamData]
		public void GetHashCodeForObjectWithComponents_SameObjectReference_Equal(TestObject obj)
		{
			var objRefCopy = obj;

			var hashCodeForObj = obj.GetObjectWithEqualityComponentsHashCode();
			var hashCodeForObjCopy = objRefCopy.GetObjectWithEqualityComponentsHashCode();

			hashCodeForObj.Should().Equals(hashCodeForObjCopy);
		}

		[Theory]
		[MendhamData]
		public void GetHashCodeForObjectWithComponents_DifferentObject_NotEqual(TestObject obj1, TestObject obj2)
		{
			var hashCodeForObj1 = obj1.GetObjectWithEqualityComponentsHashCode();
			var hashCodeForObj2 = obj2.GetObjectWithEqualityComponentsHashCode();

			hashCodeForObj1.Should().NotBe(hashCodeForObj2);
		}

		[Theory]
		[MendhamData]
		public void GetHashCodeForObjectWithComponents_DifferentObjectSameComponents_Equal(string strVal, int intVal, object objVal)
		{
			var testObj1 = new TestObject(strVal, intVal, objVal);
			var testObj2 = new TestObject(strVal, intVal, objVal);

			var hashCodeForObj1 = testObj1.GetObjectWithEqualityComponentsHashCode();
			var hashCodeForObj2 = testObj2.GetObjectWithEqualityComponentsHashCode();

			testObj1.Should().NotBeSameAs(testObj2);
			hashCodeForObj1.Should().Be(hashCodeForObj2);
		}
	}
}