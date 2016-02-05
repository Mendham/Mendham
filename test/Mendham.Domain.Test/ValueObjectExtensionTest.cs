using FluentAssertions;
using Mendham.Domain.Extensions;
using Mendham.Domain.Test.TestObjects.Other;
using Mendham.Domain.Test.TestObjects.ValueObjects.Struct;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Test
{
    public class ValueObjectExtensionTest
    {
        [Theory, MendhamData]
        public void IsEqualToValueObjectT_SameReference_True(StructValueObject valueObject)
        {
            bool result = valueObject.IsEqualToValueObject(valueObject);

            result.Should()
                .BeTrue("it is the same reference to the value object");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectT_HasDifferentValues_False(StructValueObject valueObject1, StructValueObject valueObject2)
        {
            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeFalse("the properties are not equal");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectT_HasSameValues_True(string valueObjectStr, int valueObjectInt)
        {
            var valueObject1 = new StructValueObject(valueObjectStr, valueObjectInt);
            var valueObject2 = new StructValueObject(valueObjectStr, valueObjectInt);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeTrue("the two value objects have the same properties");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectT_HasOneDifferentValue_False(string valueObject1Str, string valueObject2Str, int commonInt)
        {
            var valueObject1 = new StructValueObject(valueObject1Str, commonInt);
            var valueObject2 = new StructValueObject(valueObject2Str, commonInt);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeFalse("one of the properties of the two value objects does not match");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectT_FirstHasNull_False(string valueObject2Str, int commonInt)
        {
            var valueObject1 = new StructValueObject(null, commonInt);
            var valueObject2 = new StructValueObject(valueObject2Str, commonInt);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeFalse("one of the properties of the two value objects does not match");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectT_SecondHasNull_False(string valueObject1Str, int commonInt)
        {
            var valueObject1 = new StructValueObject(valueObject1Str, commonInt);
            var valueObject2 = new StructValueObject(null, commonInt);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeFalse("one of the properties of the two value objects does not match");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectT_MatchingValuesWithNull_True(int commonInt)
        {
            var valueObject1 = new StructValueObject(null, commonInt);
            var valueObject2 = new StructValueObject(null, commonInt);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeTrue("the properties of the two objects match");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectT_SecondObjectIsDefault(StructValueObject valueObject1)
        {
            StructValueObject valueObject2 = default(StructValueObject);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeFalse("the first value object has a value and the second is null");
        }
        [Theory, MendhamData]
        public void IsEqualToValueObjectAsObject_SameReference_True(StructValueObject valueObject)
        {
            bool result = valueObject.IsEqualToValueObject((object)valueObject);

            result.Should()
                .BeTrue("it is the same reference to the value object");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectAsObject_HasDifferentValues_False(StructValueObject valueObject1, StructValueObject valueObject2)
        {
            bool result = valueObject1.IsEqualToValueObject((object)valueObject2);

            result.Should()
                .BeFalse("the properties are not equal");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectAsObject_HasSameValues_True(string valueObjectStr, int valueObjectInt)
        {
            var valueObject1 = new StructValueObject(valueObjectStr, valueObjectInt);
            object valueObject2 = new StructValueObject(valueObjectStr, valueObjectInt);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeTrue("the two value objects have the same properties");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectAsObject_HasOneDifferentValue_False(string valueObject1Str, string valueObject2Str, int commonInt)
        {
            var valueObject1 = new StructValueObject(valueObject1Str, commonInt);
            object valueObject2 = new StructValueObject(valueObject2Str, commonInt);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeFalse("one of the properties of the two value objects does not match");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectAsObject_FirstHasNull_False(string valueObject2Str, int commonInt)
        {
            var valueObject1 = new StructValueObject(null, commonInt);
            object valueObject2 = new StructValueObject(valueObject2Str, commonInt);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeFalse("one of the properties of the two value objects does not match");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectAsObject_SecondHasNull_False(string valueObject1Str, int commonInt)
        {
            var valueObject1 = new StructValueObject(valueObject1Str, commonInt);
            object valueObject2 = new StructValueObject(null, commonInt);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeFalse("one of the properties of the two value objects does not match");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectAsObject_MatchingValuesWithNull_True(int commonInt)
        {
            var valueObject1 = new StructValueObject(null, commonInt);
            object valueObject2 = new StructValueObject(null, commonInt);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeTrue("the properties of the two objects match");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectAsObject_SecondObjectIsDefault(StructValueObject valueObject1)
        {
            object valueObject2 = default(StructValueObject);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeFalse("the first value object has a value and the second is null");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectAsObject_DifferentValueObject_False(string valueObjectStr, int valueObjectInt)
        {
            var valueObject1 = new StructValueObject(valueObjectStr, valueObjectInt);
            object valueObject2 = new AltStructValueObject(valueObjectStr, valueObjectInt);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeFalse("the two objects are not the same type of value object");
        }

        [Theory, MendhamData]
        public void IsEqualToValueObjectAsObject_NonValueObject_False(string valueObjectStr, int valueObjectInt)
        {
            var valueObject1 = new StructValueObject(valueObjectStr, valueObjectInt);
            object valueObject2 = new PlainStructWithComponents(valueObjectStr, valueObjectInt);

            bool result = valueObject1.IsEqualToValueObject(valueObject2);

            result.Should()
                .BeFalse("the second object is not a value object");
        }

        [Theory, MendhamData]
        public void GetValueObjectHashCode_SameReference_Equal(StructValueObject valueObject)
        {
            var altRefForvalueObject = valueObject;

            int expected = valueObject.GetValueObjectHashCode();
            int result = valueObject.GetValueObjectHashCode();

            result.Should()
                .Be(expected, "they have the same reference");
        }

        [Theory, MendhamData]
        public void GetValueObjectHashCode_HasDifferentValues_NotEqual(StructValueObject valueObject1, StructValueObject valueObject2)
        {
            int expected = valueObject1.GetValueObjectHashCode();
            int result = valueObject2.GetValueObjectHashCode();

            result.Should()
                .NotBe(expected, "they have a different properties");
        }

        [Theory, MendhamData]
        public void GetValueObjectHashCode_HasSameValues_Equal(string valueObjectStr, int valueObjectInt)
        {
            var valueObject1 = new StructValueObject(valueObjectStr, valueObjectInt);
            var valueObject2 = new StructValueObject(valueObjectStr, valueObjectInt);

            int expected = valueObject1.GetValueObjectHashCode();
            int result = valueObject2.GetValueObjectHashCode();

            result.Should()
                .Be(expected, "they have the same properties");
        }

        [Theory, MendhamData]
        public void GetValueObjectHashCode_HasSameValuesWithNull_Equal(int valueObjectInt)
        {
            var valueObject1 = new StructValueObject(null, valueObjectInt);
            var valueObject2 = new StructValueObject(null, valueObjectInt);

            int expected = valueObject1.GetValueObjectHashCode();
            int result = valueObject2.GetValueObjectHashCode();

            result.Should()
                .Be(expected, "they have the same properties");
        }

        [Theory, MendhamData]
        public void GetValueObjectHashCode_HasSameValues_NotEqual(string valueObjectStr, int valueObjectInt)
        {
            var valueObject1 = new StructValueObject(valueObjectStr, valueObjectInt);
            var valueObject2 = new AltStructValueObject(valueObjectStr, valueObjectInt);

            int expected = valueObject1.GetValueObjectHashCode();
            int result = valueObject2.GetValueObjectHashCode();

            result.Should()
                .NotBe(expected, "they are not the same type of value object");
        }
    }
}
