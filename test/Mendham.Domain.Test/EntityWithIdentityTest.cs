using FluentAssertions;
using Mendham.Domain.Test.TestObjects;
using Mendham.Domain.Test.TestObjects.Entities.BaseWithIdentity;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Test
{
    public class EntityWithIdentityTest
    {
        [Theory, MendhamData]
        public void EqualsT_SameReference_True(TestEntityWithIdentity entity)
        {
            bool result = entity.Equals(entity);

            result.Should()
                .BeTrue("it is the same reference to the entity");
        }

        [Theory, MendhamData]
        public void EqualsT_HasDifferentIdentity_False(TestingIdentity identity1, TestingIdentity identity2)
        {
            TestEntityWithIdentity entity1 = new TestEntityWithIdentity(identity1);
            TestEntityWithIdentity entity2 = new TestEntityWithIdentity(identity2);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("the entities do not have the same identity");
        }

        [Theory, MendhamData]
        public void EqualsT_HasSameIdentity_True(TestingIdentity identity)
        {
            TestEntityWithIdentity entity1 = new TestEntityWithIdentity(identity);
            TestEntityWithIdentity entity2 = new TestEntityWithIdentity(identity);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("the two entities have the same identity");
        }

        [Theory, MendhamData]
        public void EqualsT_HasEqualIdentity_True(string identityStr, int identityInt)
        {
            var identity1 = new TestingIdentity(identityStr, identityInt);
            var identity2 = new TestingIdentity(identityStr, identityInt);

            TestEntityWithIdentity entity1 = new TestEntityWithIdentity(identity1);
            TestEntityWithIdentity entity2 = new TestEntityWithIdentity(identity2);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("the two entities have an equal identity");
        }

        [Theory, MendhamData]
        public void EqualsT_HasUnequalIdentity_False(string identity1Str, string identity2Str, int commonInt)
        {
            var identity1 = new TestingIdentity(identity1Str, commonInt);
            var identity2 = new TestingIdentity(identity2Str, commonInt);

            TestEntityWithIdentity entity1 = new TestEntityWithIdentity(identity1);
            TestEntityWithIdentity entity2 = new TestEntityWithIdentity(identity2);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities are not equal");
        }

        [Theory, MendhamData]
        public void EqualsT_OneIdentityContainsNull_False(string identity1Str, int commonInt)
        {
            var identity1 = new TestingIdentity(identity1Str, commonInt);
            var identity2 = new TestingIdentity(null, commonInt);

            TestEntityWithIdentity entity1 = new TestEntityWithIdentity(identity1);
            TestEntityWithIdentity entity2 = new TestEntityWithIdentity(identity2);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities does not match");
        }

        [Theory, MendhamData]
        public void EqualsT_MatchingIdentityContainingNull_True(int commonInt)
        {
            var identity1 = new TestingIdentity(null, commonInt);
            var identity2 = new TestingIdentity(null, commonInt);

            TestEntityWithIdentity entity1 = new TestEntityWithIdentity(identity1);
            TestEntityWithIdentity entity2 = new TestEntityWithIdentity(identity2);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("the two entities have the same identity");
        }

        [Theory, MendhamData]
        public void EqualsT_BaseByDerivedWithCommonSharedValues_True(TestingIdentity identity)
        {
            TestEntityWithIdentity entity1 = new TestEntityWithIdentity(identity);
            TestEntityWithIdentity entity2 = new DerivedTestEntityWithIdentity(identity);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("base and derived entities with a common identity are equal when identity is equal");
        }

        [Theory, MendhamData]
        public void EqualsObject_SameReference_True(TestEntityWithIdentity entity)
        {
            bool result = entity.Equals(entity as object);

            result.Should()
                .BeTrue("it is the same reference to the entity");
        }

        [Theory, MendhamData]
        public void EqualsObject_HasDifferentIdentity_False(TestingIdentity identity1, TestingIdentity identity2)
        {
            object entity1 = new TestEntityWithIdentity(identity1);
            object entity2 = new TestEntityWithIdentity(identity2);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("the entities do not have the same identity");
        }

        [Theory, MendhamData]
        public void EqualsObject_HasSameIdentity_True(TestingIdentity identity)
        {
            object entity1 = new TestEntityWithIdentity(identity);
            object entity2 = new TestEntityWithIdentity(identity);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("the two entities have the same identity");
        }

        [Theory, MendhamData]
        public void EqualsObject_HasEqualIdentity_True(string identityStr, int identityInt)
        {
            var identity1 = new TestingIdentity(identityStr, identityInt);
            var identity2 = new TestingIdentity(identityStr, identityInt);

            object entity1 = new TestEntityWithIdentity(identity1);
            object entity2 = new TestEntityWithIdentity(identity2);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("the two entities have an equal identity");
        }

        [Theory, MendhamData]
        public void EqualsObjectHasUnequalIdentity_False(string identity1Str, string identity2Str, int commonInt)
        {
            var identity1 = new TestingIdentity(identity1Str, commonInt);
            var identity2 = new TestingIdentity(identity2Str, commonInt);

            object entity1 = new TestEntityWithIdentity(identity1);
            object entity2 = new TestEntityWithIdentity(identity2);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities are not equal");
        }

        [Theory, MendhamData]
        public void EqualsObject_OneIdentityContainsNull_False(string identity1Str, int commonInt)
        {
            var identity1 = new TestingIdentity(identity1Str, commonInt);
            var identity2 = new TestingIdentity(null, commonInt);

            object entity1 = new TestEntityWithIdentity(identity1);
            object entity2 = new TestEntityWithIdentity(identity2);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities does not match");
        }

        [Theory, MendhamData]
        public void EqualsObject_MatchingIdentityContainingNull_True(int commonInt)
        {
            var identity1 = new TestingIdentity(null, commonInt);
            var identity2 = new TestingIdentity(null, commonInt);

            object entity1 = new TestEntityWithIdentity(identity1);
            object entity2 = new TestEntityWithIdentity(identity2);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("the two entities have the same identity");
        }

        [Theory, MendhamData]
        public void EqualsObject_BaseByDerivedWithCommonSharedValues_True(TestingIdentity identity)
        {
            object entity1 = new TestEntityWithIdentity(identity);
            object entity2 = new DerivedTestEntityWithIdentity(identity);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("base and derived entities with a common identity are equal when identity is equal");
        }

        [Theory, MendhamData]
        public void EqualsObject_DerivedByBaseWithCommonSharedValues_True(TestingIdentity identity)
        {
            object entity1 = new DerivedTestEntityWithIdentity(identity);
            object entity2 = new TestEntityWithIdentity(identity);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeTrue("base and derived entities with a common identity are equal when identity is equal");
        }

        [Theory, MendhamData]
        public void EqualsObject_AltObjectWithSameIdentity_False(TestingIdentity identity)
        {
            object entity1 = new TestEntityWithIdentity(identity);
            object entity2 = new AltTestEntityIdentity(identity);

            bool result = entity1.Equals(entity2);

            result.Should()
                .BeFalse("the second entity is not of the same type");
        }

        [Theory, MendhamData]
        public void GetHashCode_SameReference_Equal(TestEntityWithIdentity entity)
        {
            var altRefForentity = entity;

            var expected = entity.GetHashCode();
            int result = entity.GetHashCode();

            result.Should()
                .Be(expected, "they have the same reference");
        }

        [Theory, MendhamData]
        public void GetHashCode_HasDifferentValues_NotEqual(TestEntityWithIdentity entity1, TestEntityWithIdentity entity2)
        {
            var expected = entity1.GetHashCode();
            int result = entity2.GetHashCode();

            result.Should()
                .NotBe(expected, "they have a different identity");
        }

        [Theory, MendhamData]
        public void GetHashCode_HasEqualIdentity_Equal(string identityStr, int identityInt)
        {
            var identity1 = new TestingIdentity(identityStr, identityInt);
            var identity2 = new TestingIdentity(identityStr, identityInt);

            TestEntityWithIdentity entity1 = new TestEntityWithIdentity(identity1);
            TestEntityWithIdentity entity2 = new TestEntityWithIdentity(identity2);

            var expected = entity1.GetHashCode();
            int result = entity2.GetHashCode();

            result.Should()
                .Be(expected, "they have an equal identity");
        }

        [Theory, MendhamData]
        public void GetHashCode_AltObjectWithSameIdentity_NotEqual(TestingIdentity identity)
        {
            var entity1 = new TestEntityWithIdentity(identity);
            var entity2 = new AltTestEntityIdentity(identity);

            var expected = entity1.GetHashCode();
            int result = entity2.GetHashCode();

            result.Should()
                .NotBe(expected, "they are not of the same type of entity");
        }

        [Theory, MendhamData]
        public void GetHashCode_DerivedTestEntityWithIdentitySameIdentity_Equal(TestingIdentity identity)
        {
            var entity1 = new TestEntityWithIdentity(identity);
            var entity2 = new DerivedTestEntityWithIdentity(identity);

            var expected = entity1.GetHashCode();
            int result = entity2.GetHashCode();

            result.Should()
                .Be(expected, "both base and derived version of the same entity share the same identity");
        }
    }
}
