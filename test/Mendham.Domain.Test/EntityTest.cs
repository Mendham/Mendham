using FluentAssertions;
using Mendham.Domain.Test.TestObjects.Entities;
using Mendham.Domain.Test.TestObjects.Other;
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
        [Theory]
        [MendhamData]
        public void EqualsT_SameReference_True(TestEntity entity)
        {
            bool result = entity.Equals(entity);

            result.Should()
                .BeTrue("it is the same reference to the entity");
        }

        [Theory]
        [MendhamData]
        public void EqualsT_HasDifferentValues_False(TestEntity entity1, TestEntity entity2)
        {
            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("the entities do not have the same identity");
        }

        [Theory]
        [MendhamData]
        public void EqualsT_HasSameValues_True(string entityStr, int entityInt)
        {
            TestEntity entity1 = new TestEntity(entityStr, entityInt);
            TestEntity entity2 = new TestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("the two entities have the same identity");
        }

        [Theory]
        [MendhamData]
        public void EqualsT_HasOneDifferentValue_False(string entity1Str, string entity2Str, int commonInt)
        {
            TestEntity entity1 = new TestEntity(entity1Str, commonInt);
            TestEntity entity2 = new TestEntity(entity2Str, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities does not match");
        }

        [Theory]
        [MendhamData]
        public void EqualsT_FirstHasNull_False(string entity2Str, int commonInt)
        {
            TestEntity entity1 = new TestEntity(null, commonInt);
            TestEntity entity2 = new TestEntity(entity2Str, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities does not match");
        }

        [Theory]
        [MendhamData]
        public void EqualsT_SecondHasNull_False(string entity1Str, int commonInt)
        {
            TestEntity entity1 = new TestEntity(entity1Str, commonInt);
            TestEntity entity2 = new TestEntity(null, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities does not match");
        }

        [Theory]
        [MendhamData]
        public void EqualsT_MatchingValuesWithNull_True(int commonInt)
        {
            TestEntity entity1 = new TestEntity(null, commonInt);
            TestEntity entity2 = new TestEntity(null, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("the two entities have the same identity");
        }

        [Theory]
        [MendhamData]
        public void EqualsT_BaseByDerivedWithCommonSharedValues_True(string entityStr, int entityInt)
        {
            TestEntity entity1 = new TestEntity(entityStr, entityInt);
            TestEntity entity2 = new DerivedTestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("both levels of the entity have identity defined at same level and are have equal identities");
        }

        [Theory]
        [MendhamData]
        public void EqualsT_BaseByDerivedOverridenIdentityWithCommonSharedAndOverridenIdentityValues_False(string entityStr, int entityInt)
        {
            TestEntity entity1 = new TestEntity(entityStr, entityInt);
            TestEntity entity2 = new DerivedOverridenIdentityTestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("the second, derived entity overrides the identity definition used by the other");
        }

        [Theory]
        [MendhamData]
        public void EqualsT_DerivedOverridenIdentityByBaseWithCommonSharedValues_False(string entityStr, int entityInt)
        {
            TestEntity entity1 = new DerivedTestEntity(entityStr, entityInt);
            TestEntity entity2 = new TestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("the first, derived entity overrides the identity definition used by the other");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_SameReference_True(TestEntity entity)
        {
            bool result = entity.Equals(entity as object);

            result.Should()
                .BeTrue("it is the same reference to the entity");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_HasDifferentValues_False(TestEntity entity1, TestEntity entity2)
        {
            bool result = entity1.Equals(entity2 as object);

            result.Should()
                .BeFalse("the entities do not have the same identity");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_HasSameValues_True(string entityStr, int entityInt)
        {
            object entity1 = new TestEntity(entityStr, entityInt);
            object entity2 = new TestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("the two entities have the same identity");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_HasOneDifferentValue_False(string entity1Str, string entity2Str, int commonInt)
        {
            object entity1 = new TestEntity(entity1Str, commonInt);
            object entity2 = new TestEntity(entity2Str, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities does not match");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_FirstHasNull_False(string entity2Str, int commonInt)
        {
            object entity1 = new TestEntity(null, commonInt);
            object entity2 = new TestEntity(entity2Str, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities does not match");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_SecondHasNull_False(string entity1Str, int commonInt)
        {
            object entity1 = new TestEntity(entity1Str, commonInt);
            object entity2 = new TestEntity(null, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities does not match");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_MatchingValuesWithNull_True(int commonInt)
        {
            object entity1 = new TestEntity(null, commonInt);
            object entity2 = new TestEntity(null, commonInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("the two entities have the same identity");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_BaseByDerivedWithCommonSharedValues_True(string entityStr, int entityInt)
        {
            object entity1 = new TestEntity(entityStr, entityInt);
            object entity2 = new DerivedTestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("both levels of the entity have identity defined at same level and are have equal identities");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_DerivedByBaseWithCommonSharedValues_True(string entityStr, int entityInt, string derivedentityStr)
        {
            object entity1 = new DerivedTestEntity(entityStr, entityInt);
            object entity2 = new TestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("both levels of the entity have identity defined at same level and are have equal identities");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_BaseByDerivedOverridenIdentityWithCommonSharedAndOverridenIdentityValues_False(string entityStr, int entityInt)
        {
            object entity1 = new TestEntity(entityStr, entityInt);
            object entity2 = new DerivedOverridenIdentityTestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("the second, derived entity overrides the identity definition used by the other");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_DerivedOverridenIdentityByBaseWithCommonSharedValues_False(string entityStr, int entityInt)
        {
            object entity1 = new DerivedOverridenIdentityTestEntity(entityStr, entityInt);
            object entity2 = new TestEntity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("the first, derived entity overrides the identity definition used by the other");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_AltObjectWithSameIdentity_False(string entityStr, int entityInt)
        {
            object entity1 = new TestEntity(entityStr, entityInt);
            object entity2 = new AltTestEntityWithSameIdentity(entityStr, entityInt);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("the second entity is not of the same type");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_BaseByNonEntityCommonSharedAndOverridenIdentityValues_False(string entityStr, int entityInt)
        {
            TestEntity entity = new TestEntity(entityStr, entityInt);
            object nonEntiy = new PlainObjectWithComponents(entityStr, entityInt);

            bool result = entity.Equals(nonEntiy);

            result.Should()
                .BeFalse("the second object does not implement IEntity");
        }

        [Theory]
        [MendhamData]
        public void EqualsObject_NonEntityByBaseCommonSharedAndOverridenIdentityValues_False(string entityStr, int entityInt)
        {
            object nonEntiy = new PlainObjectWithComponents(entityStr, entityInt);
            object entity = new TestEntity(entityStr, entityInt);

            bool result = nonEntiy.Equals(entity);

            result.Should()
                .BeFalse("Using the non entity default equals only compares by reference which is not the same");
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
        public void EqualOperator_FirstHasNull_False(string voStr, int voInt)
        {
            TestEntity entity1 = null;
            TestEntity entity2 = new TestEntity(voStr, voInt);

            bool result = entity1 == entity2;

            result.Should().BeFalse();
        }

        [Theory]
        [MendhamData]
        public void EqualOperator_SecondHasNull_False(string voStr, int voInt)
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

            result.Should()
                .Be(expected, "they have the same reference");
        }

        [Theory]
        [MendhamData]
        public void GetHashCode_HasDifferentValues_NotEqual(TestEntity entity1, TestEntity entity2)
        {
            var expected = entity1.GetHashCode();
            int result = entity2.GetHashCode();

            result.Should()
                .NotBe(expected, "they have a different identity");
        }

        [Theory]
        [MendhamData]
        public void GetHashCode_HasSameValues_Equal(string entityStr, int entityInt)
        {
            var entity1 = new TestEntity(entityStr, entityInt);
            var entity2 = new TestEntity(entityStr, entityInt);

            var expected = entity1.GetHashCode();
            int result = entity2.GetHashCode();

            result.Should()
                .Be(expected, "they have the identity");
        }

        [Theory]
        [MendhamData]
        public void GetHashCode_AltObjectWithSameIdentity_NotEqual(string entityStr, int entityInt)
        {
            var entity1 = new TestEntity(entityStr, entityInt);
            var entity2 = new AltTestEntityWithSameIdentity(entityStr, entityInt);

            var expected = entity1.GetHashCode();
            int result = entity2.GetHashCode();

            result.Should()
                .NotBe(expected, "they are not of the same type of entity");
        }

        [Theory]
        [MendhamData]
        public void GetHashCode_DerivedTestEntitySameIdentity_Equal(string entityStr, int entityInt)
        {
            var entity1 = new TestEntity(entityStr, entityInt);
            var entity2 = new DerivedTestEntity(entityStr, entityInt);

            var expected = entity1.GetHashCode();
            int result = entity2.GetHashCode();

            result.Should()
                .Be(expected, "both levels of the entity have identity defined at same level and are have equal identities");
        }

        [Theory]
        [MendhamData]
        public void GetHashCode_DerivedOverridenIdentityTestEntitySameIdentityFields_NotEqual(string entityStr, int entityInt)
        {
            var entity1 = new TestEntity(entityStr, entityInt);
            var entity2 = new DerivedOverridenIdentityTestEntity(entityStr, entityInt);

            var expected = entity1.GetHashCode();
            int result = entity2.GetHashCode();

            result.Should()
                .NotBe(expected, "the entity being tested is derived and overrides the base identity definition used by the expected");
        }
    }
}
