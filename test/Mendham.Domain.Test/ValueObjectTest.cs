using FluentAssertions;
using Mendham.Domain.Test.TestObjects.Other;
using Mendham.Domain.Test.TestObjects.ValueObjects.Base;
using Mendham.Domain.Test.TestObjects.ValueObjects.CustomEquality;
using Mendham.Domain.Test.TestObjects.ValueObjects.NoProperty;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Test
{
    public class ValueObjectTest
    {
        [Theory, MendhamData]
        public void EqualsT_SameReference_True(TestValueObject valueObject)
        {
            bool result = valueObject.Equals(valueObject);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualsT_HasDifferentValues_False(TestValueObject valueObject1, TestValueObject valueObject2)
        {
            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsT_HasSameValues_True(string voStr, int voInt)
        {
            TestValueObject valueObject1 = new TestValueObject(voStr, voInt);
            TestValueObject valueObject2 = new TestValueObject(voStr, voInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualsT_HasOneDifferentValue_False(string vo1Str, string vo2Str, int commonInt)
        {
            TestValueObject valueObject1 = new TestValueObject(vo1Str, commonInt);
            TestValueObject valueObject2 = new TestValueObject(vo2Str, commonInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsT_FirstHasNull_False(string vo2Str, int commonInt)
        {
            TestValueObject valueObject1 = new TestValueObject(null, commonInt);
            TestValueObject valueObject2 = new TestValueObject(vo2Str, commonInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsT_SecondHasNull_False(string vo1Str, int commonInt)
        {
            TestValueObject valueObject1 = new TestValueObject(vo1Str, commonInt);
            TestValueObject valueObject2 = new TestValueObject(null, commonInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsT_MatchingValuesWithNull_True(int commonInt)
        {
            TestValueObject valueObject1 = new TestValueObject(null, commonInt);
            TestValueObject valueObject2 = new TestValueObject(null, commonInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualsT_BaseByDerivedWithCommonSharedValues_False(string voStr, int voInt, string derivedVoStr)
        {
            TestValueObject valueObject1 = new TestValueObject(voStr, voInt);
            TestValueObject valueObject2 = new DerivedTestValueObject(voStr, voInt, derivedVoStr);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsT_DerivedByBaseWithCommonSharedValues_False(string voStr, int voInt, string derivedVoStr)
        {
            TestValueObject valueObject1 = new DerivedTestValueObject(voStr, voInt, derivedVoStr);
            TestValueObject valueObject2 = new TestValueObject(voStr, voInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsT_DerivedWithCommonSharedValuesOtherNull_False(string voStr, int voInt)
        {
            TestValueObject valueObject1 = new TestValueObject(voStr, voInt);
            TestValueObject valueObject2 = new DerivedTestValueObject(voStr, voInt, null);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsT_DerivedNoDifferenceWithSameFields_False(string voStr, int voInt)
        {
            TestValueObject valueObject1 = new TestValueObject(voStr, voInt);
            TestValueObject valueObject2 = new DerivedNoDifferenceTestValueObject(voStr, voInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Fact]
        public void EqualT_SameNoPropertiesValueObjectEqual_True()
        {
            NoPropertyValueObject valueObject1 = new NoPropertyValueObject();
            NoPropertyValueObject valueObject2 = new NoPropertyValueObject();

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeTrue();
        }

        [Fact]
        public void EqualT_DifferentNoPropertiesValueObjectEqual_False()
        {
            NoPropertyValueObject valueObject1 = new NoPropertyValueObject();
            AltNoPropertyValueObject valueObject2 = new AltNoPropertyValueObject();

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsT_CustomEqualitySameCustomFieldMatches_True(string voStr, int voInt1, int voInt2)
        {
            CustomEqualityComponentsValueObject valueObject1 = new CustomEqualityComponentsValueObject(voStr, voInt1);
            CustomEqualityComponentsValueObject valueObject2 = new CustomEqualityComponentsValueObject(voStr, voInt2);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualsT_CustomEqualitySameCustomFieldNoMatch_False(string voStr1, string voStr2, int voInt)
        {
            CustomEqualityComponentsValueObject valueObject1 = new CustomEqualityComponentsValueObject(voStr1, voInt);
            CustomEqualityComponentsValueObject valueObject2 = new CustomEqualityComponentsValueObject(voStr2, voInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_SameReference_True(TestValueObject valueObject)
        {
            bool result = valueObject.Equals(valueObject as object);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualsObject_HasDifferentValues_False(TestValueObject valueObject1, TestValueObject valueObject2)
        {
            bool result = valueObject1.Equals(valueObject2 as object);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_HasSameValues_True(string voStr, int voInt)
        {
            TestValueObject valueObject1 = new TestValueObject(voStr, voInt);
            object valueObject2 = new TestValueObject(voStr, voInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualsObject_HasOneDifferentValue_False(string vo1Str, string vo2Str, int commonInt)
        {
            TestValueObject valueObject1 = new TestValueObject(vo1Str, commonInt);
            object valueObject2 = new TestValueObject(vo2Str, commonInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_FirstHasNull_False(string vo2Str, int commonInt)
        {
            TestValueObject valueObject1 = new TestValueObject(null, commonInt);
            object valueObject2 = new TestValueObject(vo2Str, commonInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_SecondHasNull_False(string vo1Str, int commonInt)
        {
            TestValueObject valueObject1 = new TestValueObject(vo1Str, commonInt);
            object valueObject2 = new TestValueObject(null, commonInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_MatchingValuesWithNull_True(int commonInt)
        {
            TestValueObject valueObject1 = new TestValueObject(null, commonInt);
            object valueObject2 = new TestValueObject(null, commonInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualsObject_BaseByDerivedWithCommonSharedValues_False(string voStr, int voInt, string derivedVoStr)
        {
            TestValueObject valueObject1 = new TestValueObject(voStr, voInt);
            object valueObject2 = new DerivedTestValueObject(voStr, voInt, derivedVoStr);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_DerivedByBaseWithCommonSharedValues_False(string voStr, int voInt, string derivedVoStr)
        {
            TestValueObject valueObject1 = new DerivedTestValueObject(voStr, voInt, derivedVoStr);
            object valueObject2 = new TestValueObject(voStr, voInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_DerivedWithCommonSharedValuesOtherNull_False(string voStr, int voInt)
        {
            TestValueObject valueObject1 = new TestValueObject(voStr, voInt);
            object valueObject2 = new DerivedTestValueObject(voStr, voInt, null);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_AltObjectWithSameFields_False(string voStr, int voInt)
        {
            TestValueObject valueObject1 = new TestValueObject(voStr, voInt);
            object valueObject2 = new AltTestValueObjectWithSameFields(voStr, voInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_DerivedNoDifferenceWithSameFields_False(string voStr, int voInt)
        {
            TestValueObject valueObject1 = new TestValueObject(voStr, voInt);
            object valueObject2 = new DerivedNoDifferenceTestValueObject(voStr, voInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_NonValueObjectWithSameFields_False(string voStr, int voInt)
        {
            TestValueObject valueObject = new TestValueObject(voStr, voInt);
            object nonValueObject = new PlainObjectWithComponents(voStr, voInt);

            bool result = valueObject.Equals(nonValueObject);

            result.Should().BeFalse();
        }

        [Fact]
        public void EqualObject_SameNoPropertiesValueObjectEqual_True()
        {
            NoPropertyValueObject valueObject1 = new NoPropertyValueObject();
            object valueObject2 = new NoPropertyValueObject();

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeTrue();
        }

        [Fact]
        public void EqualObject_DifferentNoPropertiesValueObjectEqual_False()
        {
            NoPropertyValueObject valueObject1 = new NoPropertyValueObject();
            object valueObject2 = new AltNoPropertyValueObject();

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_CustomEqualitySameCustomFieldMatches_True(string voStr, int voInt1, int voInt2)
        {
            CustomEqualityComponentsValueObject valueObject1 = new CustomEqualityComponentsValueObject(voStr, voInt1);
            object valueObject2 = new CustomEqualityComponentsValueObject(voStr, voInt2);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualsObject_CustomEqualitySameCustomFieldNoMatch_False(string voStr1, string voStr2, int voInt)
        {
            CustomEqualityComponentsValueObject valueObject1 = new CustomEqualityComponentsValueObject(voStr1, voInt);
            object valueObject2 = new CustomEqualityComponentsValueObject(voStr2, voInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_CustomEqualityDifferentTypes_False(string voStr, int voInt)
        {
            CustomEqualityComponentsValueObject valueObject1 = new CustomEqualityComponentsValueObject(voStr, voInt);
            object valueObject2 = new AltCustomEqualityComponentsValueObject(voStr, voInt);

            bool result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualOperator_SameReference_True(TestValueObject valueObject)
        {
            var altRefForValueObject = valueObject;

            bool result = valueObject == altRefForValueObject;

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualOperator_HasDifferentValues_False(TestValueObject valueObject1, TestValueObject valueObject2)
        {
            bool result = valueObject1 == valueObject2;

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualOperator_HasSameValues_True(string voStr, int voInt)
        {
            var valueObject1 = new TestValueObject(voStr, voInt);
            var valueObject2 = new TestValueObject(voStr, voInt);

            bool result = valueObject1 == valueObject2;

            result.Should().BeTrue();
        }

        [Fact]
        public void EqualOperator_TwoNulls_True()
        {
            TestValueObject valueObject1 = null;
            TestValueObject valueObject2 = null;

            bool result = valueObject1 == valueObject2;

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualOperator_FirstHasNull_True(string voStr, int voInt)
        {
            TestValueObject valueObject1 = null;
            TestValueObject valueObject2 = new TestValueObject(voStr, voInt);

            bool result = valueObject1 == valueObject2;

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualOperator_SecondHasNull_True(string voStr, int voInt)
        {
            TestValueObject valueObject1 = new TestValueObject(voStr, voInt);
            TestValueObject valueObject2 = null;

            bool result = valueObject1 == valueObject2;

            result.Should().BeFalse();
        }

        [Fact]
        public void EqualOperator_SameNoPropertiesValueObjectEqual_True()
        {
            NoPropertyValueObject valueObject1 = new NoPropertyValueObject();
            NoPropertyValueObject valueObject2 = new NoPropertyValueObject();

            bool result = valueObject1 == valueObject2;

            result.Should().BeTrue();
        }

        [Fact]
        public void EqualOperator_DifferentNoPropertiesValueObjectEqual_False()
        {
            NoPropertyValueObject valueObject1 = new NoPropertyValueObject();
            AltNoPropertyValueObject valueObject2 = new AltNoPropertyValueObject();

            bool result = valueObject1 == valueObject2;

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void UnequalOperator_SameReference_False(TestValueObject valueObject)
        {
            var altRefForValueObject = valueObject;

            bool result = valueObject != altRefForValueObject;

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void UnequalOperator_HasDifferentValues_True(TestValueObject valueObject1, TestValueObject valueObject2)
        {
            bool result = valueObject1 != valueObject2;

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void UnequalOperator_HasSameValues_False(string voStr, int voInt)
        {
            var valueObject1 = new TestValueObject(voStr, voInt);
            var valueObject2 = new TestValueObject(voStr, voInt);

            bool result = valueObject1 != valueObject2;

            result.Should().BeFalse();
        }

        [Fact]
        public void UnequalOperator_TwoNulls_False()
        {
            TestValueObject valueObject1 = null;
            TestValueObject valueObject2 = null;

            bool result = valueObject1 != valueObject2;

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void UnequalOperator_FirstHasNull_True(string voStr, int voInt)
        {
            TestValueObject valueObject1 = null;
            TestValueObject valueObject2 = new TestValueObject(voStr, voInt);

            bool result = valueObject1 != valueObject2;

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void UnequalOperator_SecondHasNull_True(string voStr, int voInt)
        {
            TestValueObject valueObject1 = new TestValueObject(voStr, voInt);
            TestValueObject valueObject2 = null;

            bool result = valueObject1 != valueObject2;

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void GetHashCode_SameReference_Equal(TestValueObject valueObject)
        {
            var altRefForValueObject = valueObject;

            var expected = valueObject.GetHashCode();
            int result = valueObject.GetHashCode();

            result.Should().Be(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_HasDifferentValues_NotEqual(TestValueObject valueObject1, TestValueObject valueObject2)
        {
            var expected = valueObject1.GetHashCode();
            int result = valueObject2.GetHashCode();

            result.Should().NotBe(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_HasSameValues_Equal(string voStr, int voInt)
        {
            var valueObject1 = new TestValueObject(voStr, voInt);
            var valueObject2 = new TestValueObject(voStr, voInt);

            var expected = valueObject1.GetHashCode();
            int result = valueObject2.GetHashCode();

            result.Should().Be(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_AltObjectWithSameFields_NotEqual(string voStr, int voInt)
        {
            var valueObject1 = new TestValueObject(voStr, voInt);
            var valueObject2 = new AltTestValueObjectWithSameFields(voStr, voInt);

            var expected = valueObject1.GetHashCode();
            int result = valueObject2.GetHashCode();

            result.Should().NotBe(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_DerivedNoDifferenceWithSameFields_NotEqual(string voStr, int voInt)
        {
            var valueObject1 = new TestValueObject(voStr, voInt);
            var valueObject2 = new DerivedNoDifferenceTestValueObject(voStr, voInt);

            var expected = valueObject1.GetHashCode();
            int result = valueObject2.GetHashCode();

            result.Should().NotBe(expected);
        }

        [Fact]
        public void GetHashCode_SameNoPropertiesValueObjectEqual_Equal()
        {
            var valueObject1 = new NoPropertyValueObject();
            var valueObject2 = new NoPropertyValueObject();

            var expected = valueObject1.GetHashCode();
            int result = valueObject2.GetHashCode();

            result.Should().Be(expected);
        }

        [Fact]
        public void GetHashCode_DifferentNoPropertiesValueObjectEqual_NotEqual()
        {
            var valueObject1 = new NoPropertyValueObject();
            var valueObject2 = new AltNoPropertyValueObject();

            var expected = valueObject1.GetHashCode();
            int result = valueObject2.GetHashCode();

            result.Should().NotBe(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_CustomEqualitySameCustomFieldMatches_Equal(string voStr, int voInt1, int voInt2)
        {
            var valueObject1 = new CustomEqualityComponentsValueObject(voStr, voInt1);
            var valueObject2 = new CustomEqualityComponentsValueObject(voStr, voInt2);

            var expected = valueObject1.GetHashCode();
            int result = valueObject2.GetHashCode();

            result.Should().Be(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_CustomEqualitySameCustomFieldNoMatch_NotEqual(string voStr1, string voStr2, int voInt)
        {
            var valueObject1 = new CustomEqualityComponentsValueObject(voStr1, voInt);
            var valueObject2 = new CustomEqualityComponentsValueObject(voStr2, voInt);

            var expected = valueObject1.GetHashCode();
            int result = valueObject2.GetHashCode();

            result.Should().NotBe(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_CustomEqualityDifferentTypes_NotEqual(string voStr, int voInt)
        {
            var valueObject1 = new CustomEqualityComponentsValueObject(voStr, voInt);
            var valueObject2 = new AltCustomEqualityComponentsValueObject(voStr, voInt);

            var expected = valueObject1.GetHashCode();
            int result = valueObject2.GetHashCode();

            result.Should().NotBe(expected);
        }
    }
}
