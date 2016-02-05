using FluentAssertions;
using Mendham.Domain.Extensions;
using Mendham.Domain.Test.TestObjects.Entities.Poco;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Test
{
    public class EntityExtensionTest
    {
        [Theory, MendhamData]
        public void IsEqualToEntity_SameReference_True(PocoEntity entity)
        {
            bool result = entity.IsEqualToEntity(entity);

            result.Should()
                .BeTrue("it is the same reference to the entity");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_HasDifferentValues_False(PocoEntity entity1, PocoEntity entity2)
        {
            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeFalse("the entities do not have the same identity");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_HasSameValues_True(string entityStr, int entityInt)
        {
            IEntity entity1 = new PocoEntity(entityStr, entityInt);
            IEntity entity2 = new PocoEntity(entityStr, entityInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeTrue("the two entities have the same identity");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_HasOneDifferentValue_False(string entity1Str, string entity2Str, int commonInt)
        {
            IEntity entity1 = new PocoEntity(entity1Str, commonInt);
            IEntity entity2 = new PocoEntity(entity2Str, commonInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities does not match");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_FirstHasNull_False(string entity2Str, int commonInt)
        {
            IEntity entity1 = new PocoEntity(null, commonInt);
            IEntity entity2 = new PocoEntity(entity2Str, commonInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities does not match");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_SecondHasNull_False(string entity1Str, int commonInt)
        {
            IEntity entity1 = new PocoEntity(entity1Str, commonInt);
            IEntity entity2 = new PocoEntity(null, commonInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeFalse("part of the identity of the two entities does not match");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_MatchingValuesWithNull_True(int commonInt)
        {
            IEntity entity1 = new PocoEntity(null, commonInt);
            IEntity entity2 = new PocoEntity(null, commonInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeTrue("the two entities have the same identity");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_BaseByDerivedWithCommonSharedValuesImplicitIdentity_True(string entityStr, int entityInt)
        {
            IEntity entity1 = new PocoEntity(entityStr, entityInt);
            IEntity entity2 = new PocoDerivedEntity(entityStr, entityInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeTrue("both levels of the entity have identity defined at same level and are have equal identities");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_DerivedByBaseWithCommonSharedValuesImplicitIdentity_True(string entityStr, int entityInt, string derivedentityStr)
        {
            IEntity entity1 = new PocoDerivedEntity(entityStr, entityInt);
            IEntity entity2 = new PocoEntity(entityStr, entityInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeTrue("both levels of the entity have identity defined at same level and are have equal identities");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_BaseByDerivedOverridenIdentityWithCommonSharedAndOverridenImplicitIdentityValues_False(string entityStr, int entityInt)
        {
            IEntity entity1 = new PocoEntity(entityStr, entityInt);
            IEntity entity2 = new PocoDerivedOverridenIdentityEntity(entityStr, entityInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeFalse("the second, derived entity overrides the identity definition used by the other");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_DerivedOverridenImplicitIdentityByBaseWithCommonSharedValues_False(string entityStr, int entityInt)
        {
            IEntity entity1 = new PocoDerivedOverridenIdentityEntity(entityStr, entityInt);
            IEntity entity2 = new PocoEntity(entityStr, entityInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeFalse("the first, derived entity overrides the identity definition used by the other");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_BaseByDerivedWithCommonSharedValuesExplicitIdentity_True(string entityStr, int entityInt)
        {
            IEntity entity1 = new PocoExplicitIdentityEntity(entityStr, entityInt);
            IEntity entity2 = new PocoExplicitIdentityDerivedEntity(entityStr, entityInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeTrue("both levels of the entity have identity defined at same level and are have equal identities");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_DerivedByBaseWithCommonSharedValuesExplicitIdentity_True(string entityStr, int entityInt, string derivedentityStr)
        {
            IEntity entity1 = new PocoExplicitIdentityDerivedEntity(entityStr, entityInt);
            IEntity entity2 = new PocoExplicitIdentityEntity(entityStr, entityInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeTrue("both levels of the entity have identity defined at same level and are have equal identities");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_BaseByDerivedOverridenIdentityWithCommonSharedAndOverridenExplicitIdentityValues_False(string entityStr, int entityInt)
        {
            IEntity entity1 = new PocoExplicitIdentityEntity(entityStr, entityInt);
            IEntity entity2 = new PocoDerivedOverridenExplicitIdentityEntity(entityStr, entityInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeFalse("the second, derived entity overrides the identity definition used by the other");
        }

        [Theory, MendhamData]
        public void IsEqualToEntity_DerivedOverridenExplicitIdentityByBaseWithCommonSharedValues_False(string entityStr, int entityInt)
        {
            IEntity entity1 = new PocoDerivedOverridenExplicitIdentityEntity(entityStr, entityInt);
            IEntity entity2 = new PocoExplicitIdentityEntity(entityStr, entityInt);

            bool result = entity1.IsEqualToEntity(entity2);

            result.Should()
                .BeFalse("the first, derived entity overrides the identity definition used by the other");
        }

        [Theory, MendhamData]
        public void GetEntityHashCode_SameReference_Equal(PocoEntity entity)
        {
            IEntity altRefForentity = entity;

            int expected = entity.GetEntityHashCode();
            int result = entity.GetEntityHashCode();

            result.Should()
                .Be(expected, "they have the same reference");
        }

        [Theory, MendhamData]
        public void GetEntityHashCode_HasDifferentValues_NotEqual(PocoEntity entity1, PocoEntity entity2)
        {
            int expected = entity1.GetEntityHashCode();
            int result = entity2.GetEntityHashCode();

            result.Should()
                .NotBe(expected, "they have a different identity");
        }

        [Theory, MendhamData]
        public void GetEntityHashCode_HasSameValues_Equal(string entityStr, int entityInt)
        {
            IEntity entity1 = new PocoEntity(entityStr, entityInt);
            IEntity entity2 = new PocoEntity(entityStr, entityInt);

            int expected = entity1.GetEntityHashCode();
            int result = entity2.GetEntityHashCode();

            result.Should()
                .Be(expected, "they have the same identity");
        }

        [Theory, MendhamData]
        public void GetEntityHashCode_DerivedPocoEntitySameImplicitIdentity_Equal(string entityStr, int entityInt)
        {
            IEntity entity1 = new PocoEntity(entityStr, entityInt);
            IEntity entity2 = new PocoDerivedEntity(entityStr, entityInt);

            int expected = entity1.GetEntityHashCode();
            int result = entity2.GetEntityHashCode();

            result.Should()
                .Be(expected, "both levels of the entity have identity defined at same level and are have equal identities");
        }

        [Theory, MendhamData]
        public void GetEntityHashCode_DerivedOverridenIdentityPocoEntitySameImplicitIdentityFields_NotEqual(string entityStr, int entityInt)
        {
            IEntity entity1 = new PocoEntity(entityStr, entityInt);
            IEntity entity2 = new PocoDerivedOverridenIdentityEntity(entityStr, entityInt);

            int expected = entity1.GetEntityHashCode();
            int result = entity2.GetEntityHashCode();

            result.Should()
                .NotBe(expected, "the entity being tested is derived and overrides the base identity definition used by the expected");
        }

        [Theory, MendhamData]
        public void GetEntityHashCode_DerivedPocoEntitySameExplicitIdentity_Equal(string entityStr, int entityInt)
        {
            IEntity entity1 = new PocoExplicitIdentityEntity(entityStr, entityInt);
            IEntity entity2 = new PocoExplicitIdentityDerivedEntity(entityStr, entityInt);

            int expected = entity1.GetEntityHashCode();
            int result = entity2.GetEntityHashCode();

            result.Should()
                .Be(expected, "both levels of the entity have identity defined at same level and are have equal identities");
        }

        [Theory, MendhamData]
        public void GetEntityHashCode_DerivedOverridenIdentityPocoEntitySameExplicitIdentityFields_NotEqual(string entityStr, int entityInt)
        {
            IEntity entity1 = new PocoExplicitIdentityEntity(entityStr, entityInt);
            IEntity entity2 = new PocoDerivedOverridenExplicitIdentityEntity(entityStr, entityInt);

            int expected = entity1.GetEntityHashCode();
            int result = entity2.GetEntityHashCode();

            result.Should()
                .NotBe(expected, "the entity being tested is derived and overrides the base identity definition used by the expected");
        }
    }
}
