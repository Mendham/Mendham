using FluentAssertions;
using Mendham.Equality;
using Mendham.Test.TestObjects;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Test.Equality
{
	public class EqualityComponentsExtensionTest
	{
		[Theory]
		[MendhamData]
		public void AreComponentsEqual_SameObjectReference_Equal(BasicTestObject obj)
		{
			var objRefCopy = obj;

			var result = obj.AreComponentsEqual(objRefCopy);

			result.Should()
                .BeTrue("they are the same reference");
		}

		[Theory]
		[MendhamData]
		public void AreComponentsEqual_DifferentObject_NotEqual(BasicTestObject obj1, BasicTestObject obj2)
		{
			var result = obj1.AreComponentsEqual(obj2);

			result.Should()
                .BeFalse("they have different values");
		}

		[Theory]
		[MendhamData]
		public void AreComponentsEqual_DifferentObjectWithSameComponents_Equal(string strVal, int intVal, object objVal)
		{
			var testObj1 = new BasicTestObject(strVal, intVal, objVal);
			var testObj2 = new BasicTestObject(strVal, intVal, objVal);

			var result = testObj1.AreComponentsEqual(testObj2);

			result.Should()
                .BeTrue("they have the same components");
		}

        [Theory, MendhamData]
        public void AreComponentsEqual_EquatableComponents_Equal(string strVal)
        {
            var testObj1 = new CaseInsensitiveTestObject(strVal.ToUpper());
            var testObj2 = new CaseInsensitiveTestObject(strVal.ToLower());

            var result = testObj1.AreComponentsEqual(testObj2);

            result.Should()
                .BeTrue("they have equatable components");
        }

        [Theory]
		[MendhamData]
		public void GetHashCodeForObjectWithComponents_SameObjectReference_Equal(BasicTestObject obj)
		{
			var objRefCopy = obj;

			var expected = obj.GetObjectWithEqualityComponentsHashCode();
			var result = objRefCopy.GetObjectWithEqualityComponentsHashCode();

            result.Should()
                .Be(expected, "the same object should have the same hash value");
		}

		[Theory]
		[MendhamData]
		public void GetHashCodeForObjectWithComponents_DifferentObject_NotEqual(BasicTestObject obj1, BasicTestObject obj2)
		{
			var expected = obj1.GetObjectWithEqualityComponentsHashCode();
			var result = obj2.GetObjectWithEqualityComponentsHashCode();

			result.Should()
                .NotBe(expected, "two objects with different components will have a different hash value");
		}

		[Theory]
		[MendhamData]
		public void GetHashCodeForObjectWithComponents_DifferentObjectSameComponents_Equal(string strVal, int intVal, object objVal)
		{
			var testObj1 = new BasicTestObject(strVal, intVal, objVal);
			var testObj2 = new BasicTestObject(strVal, intVal, objVal);

			var expected = testObj1.GetObjectWithEqualityComponentsHashCode();
			var result = testObj2.GetObjectWithEqualityComponentsHashCode();

			result.Should()
                .Be(expected, "two objects with same components shoould have same hash code");
		}

        [Theory, MendhamData]
        public void GetHashCodeForObjectWithComponents_EquatableComponents_Equal(string strVal)
        {
            var testObj1 = new CaseInsensitiveTestObject(strVal.ToUpper());
            var testObj2 = new CaseInsensitiveTestObject(strVal.ToLower());

            var expected = testObj1.GetObjectWithEqualityComponentsHashCode();
            var result = testObj2.GetObjectWithEqualityComponentsHashCode();

            result.Should()
                .Be(expected, "two objects with same components that are equtable shoould have same hash code");
        }
    }
}