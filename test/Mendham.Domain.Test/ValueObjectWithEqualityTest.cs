using FluentAssertions;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Test
{
    public class ValueObjectWithEqualityTest
    {
        public class TestValueObject : ValueObject<TestValueObject>
        {
            public string StrVal { get; private set; }
            public int IntVal { get; private set; }

            public TestValueObject(string strVal, int intVal)
            {
                this.StrVal = strVal;
                this.IntVal = intVal;
            }
        }

        public class DerivedTestValueObject : TestValueObject
        {
            public string DerivedStrVal { get; private set; }

            public DerivedTestValueObject(string strVal, int intVal, string derivedStrVal) : base(strVal, intVal)
            {
                this.DerivedStrVal = derivedStrVal;
            }
        }

        public class AltTestValueObjectWithSameFields : ValueObject<AltTestValueObjectWithSameFields>
        {
            public string StrVal { get; private set; }
            public int IntVal { get; private set; }

            public AltTestValueObjectWithSameFields(string strVal, int intVal)
            {
                this.StrVal = strVal;
                this.IntVal = intVal;
            }
        }

        [Theory]
        [MendhamData]
        public void Equals_SameReference_True(TestValueObject valueObject)
        {
            var result = valueObject.Equals(valueObject);

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void Equals_HasDifferentValues_False(TestValueObject valueObject1, TestValueObject valueObject2)
        {
            var result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void Equals_HasSameValues_True(string voStr, int voInt)
        {
            var valueObject1 = new TestValueObject(voStr, voInt);
            var valueObject2 = new TestValueObject(voStr, voInt);

            var result = valueObject1.Equals(valueObject2);

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void Equals_HasOneDifferentValue_False(string vo1Str, string vo2Str, int commonInt)
        {
            var valueObject1 = new TestValueObject(vo1Str, commonInt);
            var valueObject2 = new TestValueObject(vo2Str, commonInt);

            var result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void Equals_FirstHasNull_False(string vo2Str, int commonInt)
        {
            var valueObject1 = new TestValueObject(null, commonInt);
            var valueObject2 = new TestValueObject(vo2Str, commonInt);

            var result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void Equals_SecondHasNull_False(string vo1Str, int commonInt)
        {
            var valueObject1 = new TestValueObject(vo1Str, commonInt);
            var valueObject2 = new TestValueObject(null, commonInt);

            var result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void Equals_MatchingValuesWithNull_True(int commonInt)
        {
            var valueObject1 = new TestValueObject(null, commonInt);
            var valueObject2 = new TestValueObject(null, commonInt);

            var result = valueObject1.Equals(valueObject2);

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void Equals_BaseByDerivedWithCommonSharedValues_False(string voStr, int voInt, string derivedVoStr)
        {
            var valueObject1 = new TestValueObject(voStr, voInt);
            var valueObject2 = new DerivedTestValueObject(voStr, voInt, derivedVoStr);

            var result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void Equals_DerivedByBaseWithCommonSharedValues_False(string voStr, int voInt, string derivedVoStr)
        {
            var valueObject1 = new DerivedTestValueObject(voStr, voInt, derivedVoStr);
            var valueObject2 = new TestValueObject(voStr, voInt);

            var result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void Equals_DerivedWithCommonSharedValuesOtherNull_False(string voStr, int voInt)
        {
            var valueObject1 = new TestValueObject(voStr, voInt);
            var valueObject2 = new DerivedTestValueObject(voStr, voInt, null);

            var result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void Equals_AltObjectWithSameFields_False(string voStr, int voInt)
        {
            var valueObject1 = new TestValueObject(voStr, voInt);
            var valueObject2 = new AltTestValueObjectWithSameFields(voStr, voInt);

            var result = valueObject1.Equals(valueObject2);

            result.Should().BeFalse();
        }
    }
}
