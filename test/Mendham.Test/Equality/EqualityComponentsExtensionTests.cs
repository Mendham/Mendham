﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBeall.Mendham.Equality;
using FluentAssertions;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace CBeall.Mendham.Test.Equality
{
	public class EqualityComponentsExtensionTests
	{
		public class TestObject : IHasEqualityComponents
		{
			public TestObject(String strVal, int intVal, object objVal)
			{
				this.StrVal = strVal;
				this.IntVal = intVal;
				this.ObjVal = objVal;
			}

			public String StrVal { get; set; }
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
		[AutoData]
		public void SameObjectReferenceIsEqual(TestObject obj)
		{
			var objRefCopy = obj;

			var equal = obj.EqualsFromComponents(objRefCopy);

			equal.Should().BeTrue();
		}

		[Theory]
		[AutoData]
		public void DifferentObjectNotEqual(TestObject obj1, TestObject obj2)
		{
			var equal = obj1.EqualsFromComponents(obj2);

			equal.Should().BeFalse();
		}

		[Theory]
		[AutoData]
		public void DifferentObjectSameComponentsEqual(String strVal, int intVal, object objVal)
		{
			var testObj1 = new TestObject(strVal, intVal, objVal);
			var testObj2 = new TestObject(strVal, intVal, objVal);

			var equal = testObj1.EqualsFromComponents(testObj2);

			testObj1.Should().NotBeSameAs(testObj2);
			equal.Should().BeTrue();
		}

		[Theory]
		[AutoData]
		public void SameObjectReferenceSameHashcode(TestObject obj)
		{
			var objRefCopy = obj;

			var hashCodeForObj = obj.GetHashCodeFromComponents();
			var hashCodeForObjCopy = objRefCopy.GetHashCodeFromComponents();

			hashCodeForObj.Should().Equals(hashCodeForObjCopy);
		}

		[Theory]
		[AutoData]
		public void DifferentObjectDifferentHashCode(TestObject obj1, TestObject obj2)
		{
			var hashCodeForObj1 = obj1.GetHashCodeFromComponents();
			var hashCodeForObj2 = obj2.GetHashCodeFromComponents();

			hashCodeForObj1.Should().NotBe(hashCodeForObj2);
		}

		[Theory]
		[AutoData]
		public void DifferentObjectSameComponentsSameHashCode(String strVal, int intVal, object objVal)
		{
			var testObj1 = new TestObject(strVal, intVal, objVal);
			var testObj2 = new TestObject(strVal, intVal, objVal);

			var hashCodeForObj1 = testObj1.GetHashCodeFromComponents();
			var hashCodeForObj2 = testObj2.GetHashCodeFromComponents();

			testObj1.Should().NotBeSameAs(testObj2);
			hashCodeForObj1.Should().Be(hashCodeForObj2);
		}
	}
}