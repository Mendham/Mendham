using FluentAssertions;
using Mendham.Equality;
using Mendham.Test.TestObjects;
using Mendham.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Test.Equality
{
    public class HasEqualityComponentsComparerTest
    {
        [Theory, MendhamData]
        public void Equals_SameReferenceObject_True(object obj)
        {
            var altObjRef = obj;

            var sut = HasEqualityComponentsComparer.Default;

            var result = sut.Equals(obj, altObjRef);

            result.Should()
                .BeTrue("objects are the same reference");
        }

        [Theory, MendhamData]
        public void Equals_TwoEqualObjecst_True(string part1, string part2)
        {
            // Done to generate two difference references
            var str1 = part1 + part2;
            var str2 = part1 + part2;

            var sut = HasEqualityComponentsComparer.Default;

            var result = sut.Equals(str1, str2);

            str1.Should()
                .NotBeSameAs(str2, "this is the test condition");
            result.Should()
                .BeTrue("objects are equal");
        }

        [Theory, MendhamData]
        public void Equals_NotEqualObject_False(string str1, string str2)
        {
            var sut = HasEqualityComponentsComparer.Default;

            var result = sut.Equals(str1, str2);

            result.Should()
                .BeFalse("objects are not equal");
        }

        [Theory]
        [InlineMendhamData(true)]
        [InlineMendhamData(false)]
        public void Equals_ComponentWithComparer_Expected(bool expected)
        {
            var cwc1Mock = new Mock<IComponentWithComparer>();
            var cwc2Mock = new Mock<IComponentWithComparer>();

            var cwc1 = cwc1Mock.Object;
            var cwc2 = cwc2Mock.Object;

            cwc1Mock.Setup(a => a.IsEqualToComponent(cwc2)).Returns(expected);

            var sut = HasEqualityComponentsComparer.Default;
            var result = sut.Equals(cwc1, cwc2);

            result.Should()
                .Be(expected, "that is the result from the IComponentWithComparer");
        }

        [Theory, MendhamData]
        public void Equals_ObjectsHaveEqualComponents_True(string strVal, int intVal, object objVal)
        {
            var testObject1 = new BasicTestObject(strVal, intVal, objVal);
            var testObject2 = new BasicTestObject(strVal, intVal, objVal);

            var sut = HasEqualityComponentsComparer.Default;
            var result = sut.Equals(testObject1, testObject2);

            result.Should()
                .BeTrue("the components of the two objects are equal and are of the same type");
        }

        [Theory, MendhamData]
        public void Equals_ObjectsHaveEqualComponents_False(string strVal1, string strVal2, int intVal, object objVal)
        {
            var testObject1 = new BasicTestObject(strVal1, intVal, objVal);
            var testObject2 = new BasicTestObject(strVal2, intVal, objVal);

            var sut = HasEqualityComponentsComparer.Default;
            var result = sut.Equals(testObject1, testObject2);

            result.Should()
                .BeFalse("the components of the two objects are not equal");
        }

        [Theory, MendhamData]
        public void Equals_ObjectsHaveEqualComponentsButDifferentTypes_False(string strVal, int intVal, object objVal)
        {
            var testObject1 = new BasicTestObject(strVal, intVal, objVal);
            var testObject2 = new AltTestObject(strVal, intVal, objVal);

            var sut = HasEqualityComponentsComparer.Default;
            var result = sut.Equals(testObject1, testObject2);

            result.Should()
                .BeFalse("the components are not of the same type");
        }

        [Theory, MendhamData]
        public void GetHashCode_SameReferenceObject_Equal(object obj)
        {
            var altObjRef = obj;

            var sut = HasEqualityComponentsComparer.Default;

            var expected = sut.GetHashCode(obj);
            var result = sut.GetHashCode(altObjRef);

            result.Should()
                .Be(expected, "objects with the same reference have the same hash code");
        }

        [Theory, MendhamData]
        public void GetHashCode_TwoEqualObjecst_Equal(string part1, string part2)
        {
            // Done to generate two difference references
            var str1 = part1 + part2;
            var str2 = part1 + part2;

            var sut = HasEqualityComponentsComparer.Default;

            var expected = sut.GetHashCode(str1);
            var result = sut.GetHashCode(str2);

            str1.Should()
                .NotBeSameAs(str2, "this is the test condition");
            result.Should()
                .Be(expected, "objects are equal");
        }

        [Theory, MendhamData]
        public void GetHashCode_NotEqualObject_NotEqual(string str1, string str2)
        {
            var sut = HasEqualityComponentsComparer.Default;

            var expected = sut.GetHashCode(str1);
            var result = sut.GetHashCode(str2);

            result.Should()
                .NotBe(expected, "objects are not equal");
        }

        [Fact]
        public void GetHashCode_TwoNullObjecst_Equal()
        {
            object obj1 = null;
            object obj2 = null;

            var sut = HasEqualityComponentsComparer.Default;

            var expected = sut.GetHashCode(obj1);
            var result = sut.GetHashCode(obj2);

            result.Should()
                .Be(expected, "nulls return the same object")
                .And.NotBe(default(int), "it should not be the default int");
        }

        [Theory, MendhamData]
        public void GetHashCode_ComponentWithComparer_Expected(int expectedHashCode)
        {
            var cwc = Mock.Of<IComponentWithComparer>(ctx =>
                ctx.GetComponentHashCode() == expectedHashCode);

            var sut = HasEqualityComponentsComparer.Default;

            var result = sut.GetHashCode(cwc);

            result.Should()
                .Be(expectedHashCode, "that is the result return by the IComponentWithComparer");
        }

        [Theory, MendhamData]
        public void GetHashCode_ObjectsHaveEqualComponents_Equal(string strVal, int intVal, object objVal)
        {
            var testObject1 = new BasicTestObject(strVal, intVal, objVal);
            var testObject2 = new BasicTestObject(strVal, intVal, objVal);

            var sut = HasEqualityComponentsComparer.Default;

            var expected = sut.GetHashCode(testObject1);
            var result = sut.GetHashCode(testObject2);

            result.Should()
                .Be(expected, "the components of the two objects are equal and are of the same type therefore hash code is equal");
        }

        [Theory, MendhamData]
        public void GetHashCode_ObjectsHaveEqualComponents_NotEqual(string strVal1, string strVal2, int intVal, object objVal)
        {
            var testObject1 = new BasicTestObject(strVal1, intVal, objVal);
            var testObject2 = new BasicTestObject(strVal2, intVal, objVal);

            var sut = HasEqualityComponentsComparer.Default;

            var expected = sut.GetHashCode(testObject1);
            var result = sut.GetHashCode(testObject2);

            result.Should()
                .NotBe(expected, "the components of the two objects are not equal and therefore the objects have different hash codes");
        }

        [Theory, MendhamData]
        public void GetHashCode_ObjectsHaveEqualComponentsButDifferentTypes_NotEqual(string strVal, int intVal, object objVal)
        {
            var testObject1 = new BasicTestObject(strVal, intVal, objVal);
            var testObject2 = new AltTestObject(strVal, intVal, objVal);

            var sut = HasEqualityComponentsComparer.Default;

            var expected = sut.GetHashCode(testObject1);
            var result = sut.GetHashCode(testObject2);

            result.Should()
                .NotBe(expected, "the components are not of the same type and therefore have different hash codes");
        }
    }
}
