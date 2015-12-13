using FluentAssertions;
using Mendham.Domain.Extensions;
using Mendham.Domain.Test.TestObjects;
using Mendham.Domain.Test.TestObjects.Entities.PocoWithIdentity;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Test
{
    public class EntityExtensionForEntityWithIdentityTest
    {
        [Theory]
        [MendhamData]
        public void IsEqualToEntity_SameReference_True(PocoWithIdentityEntity entity)
        {
            bool result = entity.IsEqualToEntity(entity);

            result.Should()
                .BeTrue("it is the same reference to the entity");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToEntity_HasDifferentIdentity_False(TestingIdentity identity1, TestingIdentity identity2)
        {
            IEntity entity1 = new PocoWithIdentityEntity(identity1);
            IEntity entity2 = new PocoWithIdentityEntity(identity2);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeFalse("the entities do not have the same identity");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToEntity_HasSameIdentity_True(TestingIdentity identity)
        {
            IEntity entity1 = new PocoWithIdentityEntity(identity);
            IEntity entity2 = new PocoWithIdentityEntity(identity);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeTrue("the two entities have the same identity");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToEntity_HasEqualIdentity_True(string identityStr, int identityInt)
        {
            var identity1 = new TestingIdentity(identityStr, identityInt);
            var identity2 = new TestingIdentity(identityStr, identityInt);

            IEntity entity1 = new PocoWithIdentityEntity(identity1);
            IEntity entity2 = new PocoWithIdentityEntity(identity2);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeTrue("the two entities have an equal identity");
        }

        [Theory]
        [MendhamData]
        public void EqualsObjectHasUnequalIdentity_False(string identity1Str, string identity2Str, int commonInt)
        {
            var identity1 = new TestingIdentity(identity1Str, commonInt);
            var identity2 = new TestingIdentity(identity2Str, commonInt);

            IEntity entity1 = new PocoWithIdentityEntity(identity1);
            IEntity entity2 = new PocoWithIdentityEntity(identity2);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities are not equal");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToEntity_OneIdentityContainsNull_False(string identity1Str, int commonInt)
        {
            var identity1 = new TestingIdentity(identity1Str, commonInt);
            var identity2 = new TestingIdentity(null, commonInt);

            IEntity entity1 = new PocoWithIdentityEntity(identity1);
            IEntity entity2 = new PocoWithIdentityEntity(identity2);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities does not match");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToEntity_MatchingIdentityContainingNull_True(int commonInt)
        {
            var identity1 = new TestingIdentity(null, commonInt);
            var identity2 = new TestingIdentity(null, commonInt);

            IEntity entity1 = new PocoWithIdentityEntity(identity1);
            IEntity entity2 = new PocoWithIdentityEntity(identity2);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeTrue("the two entities have the same identity");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToEntity_BaseByDerivedWithCommonSharedValues_True(TestingIdentity identity)
        {
            IEntity entity1 = new PocoWithIdentityEntity(identity);
            IEntity entity2 = new PocoWithIdentityDerivedEntity(identity);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeTrue("base and derived entities with a common identity are equal when identity is equal");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToEntity_DerivedByBaseWithCommonSharedValues_True(TestingIdentity identity)
        {
            IEntity entity1 = new PocoWithIdentityDerivedEntity(identity);
            IEntity entity2 = new PocoWithIdentityEntity(identity);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeTrue("base and derived entities with a common identity are equal when identity is equal");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToEntity_AltObjectWithSameIdentity_False(TestingIdentity identity)
        {
            IEntity entity1 = new PocoWithIdentityEntity(identity);
            IEntity entity2 = new AltPocoWithIdentityEntity(identity);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeFalse("the second entity is not of the same type");
        }

        [Theory]
        [MendhamData]
        public void GetEntityHashCode_SameReference_Equal(PocoWithIdentityEntity entity)
        {
            var altRefForentity = entity;

            var expected = entity.GetEntityHashCode();
            int result = entity.GetEntityHashCode();

            result.Should()
                .Be(expected, "they have the same reference");
        }

        [Theory]
        [MendhamData]
        public void GetEntityHashCode_HasDifferentValues_NotEqual(PocoWithIdentityEntity entity1, PocoWithIdentityEntity entity2)
        {
            var expected = entity1.GetEntityHashCode();
            int result = entity2.GetEntityHashCode();

            result.Should()
                .NotBe(expected, "they have a different identity");
        }

        [Theory]
        [MendhamData]
        public void GetEntityHashCode_HasEqualIdentity_Equal(string identityStr, int identityInt)
        {
            var identity1 = new TestingIdentity(identityStr, identityInt);
            var identity2 = new TestingIdentity(identityStr, identityInt);

            IEntity entity1 = new PocoWithIdentityEntity(identity1);
            IEntity entity2 = new PocoWithIdentityEntity(identity2);

            var expected = entity1.GetEntityHashCode();
            int result = entity2.GetEntityHashCode();

            result.Should()
                .Be(expected, "they have an equal identity");
        }

        [Theory]
        [MendhamData]
        public void GetEntityHashCode_AltObjectWithSameIdentity_NotEqual(TestingIdentity identity)
        {
            IEntity entity1 = new PocoWithIdentityEntity(identity);
            IEntity entity2 = new AltPocoWithIdentityEntity(identity);

            var expected = entity1.GetEntityHashCode();
            int result = entity2.GetEntityHashCode();

            result.Should()
                .NotBe(expected, "they are not of the same type of entity");
        }

        [Theory]
        [MendhamData]
        public void GetEntityHashCode_PocoWithIdentityDerivedEntitySameIdentity_Equal(TestingIdentity identity)
        {
            IEntity entity1 = new PocoWithIdentityEntity(identity);
            IEntity entity2 = new PocoWithIdentityDerivedEntity(identity);

            var expected = entity1.GetEntityHashCode();
            int result = entity2.GetEntityHashCode();

            result.Should()
                .Be(expected, "both base and derived version of the same entity share the same identity");
        }
    }
}
