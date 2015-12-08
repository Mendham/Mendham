using FluentAssertions;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Test
{
    public class EntityTest
    {
        public class TestEntity : Entity<TestEntity>
        {
            public string StrVal { get; private set; }
            public int IntVal { get; private set; }
            public Guid nonIdentityValue { get; set; }

            public TestEntity(string strVal, int intVal)
            {
                this.StrVal = strVal;
                this.IntVal = intVal;
                nonIdentityValue = Guid.NewGuid();
            }

            protected override IEnumerable<object> IdentityComponents
            {
                get
                {
                    yield return StrVal;
                    yield return IntVal;
                }
            }
        }

        public class DerivedTestEntity : TestEntity
        {
            public string DerivedStrVal { get; private set; }

            public DerivedTestEntity(string strVal, int intVal, string derivedStrVal) : base(strVal, intVal)
            {
                this.DerivedStrVal = derivedStrVal;
            }
        }

        public class AltTestEntityWithSameFields : Entity<AltTestEntityWithSameFields>
        {
            public string StrVal { get; private set; }
            public int IntVal { get; private set; }
            public Guid nonIdentityValue { get; set; }

            public AltTestEntityWithSameFields(string strVal, int intVal)
            {
                this.StrVal = strVal;
                this.IntVal = intVal;
                nonIdentityValue = Guid.NewGuid();
            }

            protected override IEnumerable<object> IdentityComponents
            {
                get
                {
                    yield return StrVal;
                    yield return IntVal;
                }
            }
        }

        [Theory]
        [MendhamData]
        public void EqualsT_SameReference_True(TestEntity entity)
        {
            bool result = entity.Equals(entity);

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void EqualsT_HasDifferentValues_False(TestEntity entity1, TestEntity entity2)
        {
            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsT_HasSameValues_True(string entityStr, int entityInt)
        {
            TestEntity entity1 = new TestEntity(entityStr, entityInt);
            TestEntity entity2 = new TestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void EqualsT_HasOneDifferentValue_False(string entity1Str, string entity2Str, int commonInt)
        {
            TestEntity entity1 = new TestEntity(entity1Str, commonInt);
            TestEntity entity2 = new TestEntity(entity2Str, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsT_FirstHasNull_False(string entity2Str, int commonInt)
        {
            TestEntity entity1 = new TestEntity(null, commonInt);
            TestEntity entity2 = new TestEntity(entity2Str, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsT_SecondHasNull_False(string entity1Str, int commonInt)
        {
            TestEntity entity1 = new TestEntity(entity1Str, commonInt);
            TestEntity entity2 = new TestEntity(null, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsT_MatchingValuesWithNull_True(int commonInt)
        {
            TestEntity entity1 = new TestEntity(null, commonInt);
            TestEntity entity2 = new TestEntity(null, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void EqualsT_BaseByDerivedWithCommonSharedValues_False(string entityStr, int entityInt, string derivedentityStr)
        {
            TestEntity entity1 = new TestEntity(entityStr, entityInt);
            TestEntity entity2 = new DerivedTestEntity(entityStr, entityInt, derivedentityStr);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsT_DerivedByBaseWithCommonSharedValues_False(string entityStr, int entityInt, string derivedentityStr)
        {
            TestEntity entity1 = new DerivedTestEntity(entityStr, entityInt, derivedentityStr);
            TestEntity entity2 = new TestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsT_DerivedWithCommonSharedValuesOtherNull_False(string entityStr, int entityInt)
        {
            TestEntity entity1 = new TestEntity(entityStr, entityInt);
            TestEntity entity2 = new DerivedTestEntity(entityStr, entityInt, null);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_SameReference_True(TestEntity entity)
        {
            bool result = entity.Equals(entity as object);

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_HasDifferentValues_False(TestEntity entity1, TestEntity entity2)
        {
            bool result = entity1.Equals(entity2 as object);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_HasSameValues_True(string entityStr, int entityInt)
        {
            TestEntity entity1 = new TestEntity(entityStr, entityInt);
            object entity2 = new TestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_HasOneDifferentValue_False(string entity1Str, string entity2Str, int commonInt)
        {
            TestEntity entity1 = new TestEntity(entity1Str, commonInt);
            object entity2 = new TestEntity(entity2Str, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_FirstHasNull_False(string entity2Str, int commonInt)
        {
            TestEntity entity1 = new TestEntity(null, commonInt);
            object entity2 = new TestEntity(entity2Str, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_SecondHasNull_False(string entity1Str, int commonInt)
        {
            TestEntity entity1 = new TestEntity(entity1Str, commonInt);
            object entity2 = new TestEntity(null, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_MatchingValuesWithNull_True(int commonInt)
        {
            TestEntity entity1 = new TestEntity(null, commonInt);
            object entity2 = new TestEntity(null, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_BaseByDerivedWithCommonSharedValues_False(string entityStr, int entityInt, string derivedentityStr)
        {
            TestEntity entity1 = new TestEntity(entityStr, entityInt);
            object entity2 = new DerivedTestEntity(entityStr, entityInt, derivedentityStr);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_DerivedByBaseWithCommonSharedValues_False(string entityStr, int entityInt, string derivedentityStr)
        {
            TestEntity entity1 = new DerivedTestEntity(entityStr, entityInt, derivedentityStr);
            object entity2 = new TestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_DerivedWithCommonSharedValuesOtherNull_False(string entityStr, int entityInt)
        {
            TestEntity entity1 = new TestEntity(entityStr, entityInt);
            object entity2 = new DerivedTestEntity(entityStr, entityInt, null);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_AltObjectWithSameFields_False(string entityStr, int entityInt)
        {
            TestEntity entity1 = new TestEntity(entityStr, entityInt);
            object entity2 = new AltTestEntityWithSameFields(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualOperator_SameReference_True(TestEntity entity)
        {
            var altRefForentity = entity;

            bool result = entity == altRefForentity;

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void EqualOperator_HasDifferentValues_False(TestEntity entity1, TestEntity entity2)
        {
            bool result = entity1 == entity2;

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualOperator_HasSameValues_True(string entityStr, int entityInt)
        {
            var entity1 = new TestEntity(entityStr, entityInt);
            var entity2 = new TestEntity(entityStr, entityInt);

            bool result = entity1 == entity2;

            result.Should().BeTrue();
        }

        [Fact]
        public void EqualOperator_TwoNulls_True()
        {
            TestEntity entity1 = null;
            TestEntity entity2 = null;

            bool result = entity1 == entity2;

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void EqualOperator_FirstHasNull_True(string voStr, int voInt)
        {
            TestEntity entity1 = null;
            TestEntity entity2 = new TestEntity(voStr, voInt);

            bool result = entity1 == entity2;

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualOperator_SecondHasNull_True(string voStr, int voInt)
        {
            TestEntity entity1 = new TestEntity(voStr, voInt);
            TestEntity entity2 = null;

            bool result = entity1 == entity2;

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void UnequalOperator_SameReference_False(TestEntity entity)
        {
            var altRefForentity = entity;

            bool result = entity != altRefForentity;

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void UnequalOperator_HasDifferentValues_True(TestEntity entity1, TestEntity entity2)
        {
            bool result = entity1 != entity2;

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void UnequalOperator_HasSameValues_False(string entityStr, int entityInt)
        {
            var entity1 = new TestEntity(entityStr, entityInt);
            var entity2 = new TestEntity(entityStr, entityInt);

            bool result = entity1 != entity2;

            result.Should().BeFalse();
        }

        [Fact]
        public void UnequalOperator_TwoNulls_False()
        {
            TestEntity entity1 = null;
            TestEntity entity2 = null;

            bool result = entity1 != entity2;

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void UnequalOperator_FirstHasNull_True(string voStr, int voInt)
        {
            TestEntity entity1 = null;
            TestEntity entity2 = new TestEntity(voStr, voInt);

            bool result = entity1 != entity2;

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void UnequalOperator_SecondHasNull_True(string voStr, int voInt)
        {
            TestEntity entity1 = new TestEntity(voStr, voInt);
            TestEntity entity2 = null;

            bool result = entity1 != entity2;

            result.Should().BeTrue();
        }

        [Theory]
        [MendhamData]
        public void GetHashCode_SameReference_Equal(TestEntity entity)
        {
            var altRefForentity = entity;

            var expected = entity.GetHashCode();
            int result = entity.GetHashCode();

            result.Should().Be(expected);
        }

        [Theory]
        [MendhamData]
        public void GetHashCode_HasDifferentValues_NotEqual(TestEntity entity1, TestEntity entity2)
        {
            var expected = entity1.GetHashCode();
            int result = entity2.GetHashCode();

            result.Should().NotBe(expected);
        }

        [Theory]
        [MendhamData]
        public void GetHashCode_HasSameValues_Equal(string entityStr, int entityInt)
        {
            var entity1 = new TestEntity(entityStr, entityInt);
            var entity2 = new TestEntity(entityStr, entityInt);

            var expected = entity1.GetHashCode();
            int result = entity2.GetHashCode();

            result.Should().Be(expected);
        }

        [Theory]
        [MendhamData]
        public void GetHashCode_AltObjectWithSameFields_NotEqual(string entityStr, int entityInt)
        {
            var entity1 = new TestEntity(entityStr, entityInt);
            var entity2 = new AltTestEntityWithSameFields(entityStr, entityInt);

            var expected = entity1.GetHashCode();
            int result = entity2.GetHashCode();

            result.Should().NotBe(expected);
        }
    }
}
