using FluentAssertions;
using Mendham.Testing.Builder.Exceptions;
using Mendham.Testing.Builder.Test.Fixtures;
using Mendham.Testing.Builder.Test.TestObjects;
using Mendham.Testing.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test
{
    public class BuilderRegistrationTest : BaseUnitTest<BuilderRegistrationFixture>
    {
        public BuilderRegistrationTest(BuilderRegistrationFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Register_BuilderTypeDoesNotImplementIBuilder_ThrowsInvalidBuilderException()
        {
            var classThatDoesNotImplementIBuilder = typeof(ClassThatDoesNotImplementIBuilder);
            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(classThatDoesNotImplementIBuilder);

            var sut = TestFixture.CreateSut();
            Action act = () => sut.Register(TestFixture.TestAssembly);

            act.ShouldThrow<InvalidBuilderException>()
                .Where(a => a.BuilderType == classThatDoesNotImplementIBuilder);
        }

        [Fact]
        public void Register_DataBuilderOverrideMismatch_ThrowsInvalidMendhamBuilderOverrideException()
        {
            var builderType = typeof(ConstrainedInputObjectBuilder);
            var typeOverriden = typeof(DerivedConstrainedInputObject);
            var defaultTypeBuilderByBuilder = typeof(ConstrainedInputObject);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute(typeOverriden);

            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(builderType);
            TestFixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderType))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = TestFixture.CreateSut();
            Action act = () => sut.Register(TestFixture.TestAssembly);

            act.ShouldThrow<InvalidMendhamBuilderOverrideException>()
                .Where(a => a.BuilderType == builderType
                    && a.TypeOverride == typeOverriden
                    && a.TypeToBuild == defaultTypeBuilderByBuilder);
        }

        [Fact]
        public void Register_MultipleBuildersAssignedToType_ThrowsMultipleBuilderForTypeException()
        {
            var firstBuilder = typeof(ConstrainedInputObjectBuilder);
            var secondBuilder = typeof(AdditionalConstrainedInputObjectBuilder);
            var defaultTypeBuilderByBuilder = typeof(ConstrainedInputObject);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute();

            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(firstBuilder, secondBuilder);
            TestFixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(It.IsAny<Type>()))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = TestFixture.CreateSut();
            Action act = () => sut.Register(TestFixture.TestAssembly);

            act.ShouldThrow<MultipleBuilderForTypeException>()
                .Where(a => a.BuilderType == firstBuilder
                    && a.AdditionalBuilderType == secondBuilder
                    && a.TypeToBuild == defaultTypeBuilderByBuilder);
        }

        [Fact]
        public void IsBuilderForTypeRegistered_SingleDefaultBuilder_True()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute();

            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            TestFixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = TestFixture.CreateSut();
            sut.Register(TestFixture.TestAssembly);

            var result = sut.IsTypeRegistered<ConstrainedInputObject>();

            result.Should()
                .BeTrue();
        }

        [Fact]
        public void IsTypeRegistered_SingleOverridenBuilder_True()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var abstractConstrainedInputObjectType = typeof(AbstractConstrainedInputObject);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute(abstractConstrainedInputObjectType);

            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            TestFixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = TestFixture.CreateSut();
            sut.Register(TestFixture.TestAssembly);

            var result = sut.IsTypeRegistered<AbstractConstrainedInputObject>();

            result.Should()
                .BeTrue();
        }

        [Fact]
        public void IsTypeRegistered_TypeNotRegistered_False()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute();

            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            TestFixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = TestFixture.CreateSut();
            sut.Register(TestFixture.TestAssembly);

            var result = sut.IsTypeRegistered<DerivedConstrainedInputObject>();

            result.Should()
                .BeFalse();
        }

        [Fact]
        public void IsTypeRegistered_MultipleTypesForBuilder_AllTrue()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var defaultMendhamBuilderAttribute = new MendhamBuilderAttribute();
            var overridingMendhamBuilderAttribute = new MendhamBuilderAttribute(typeof(AbstractConstrainedInputObject));

            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            TestFixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(defaultMendhamBuilderAttribute, overridingMendhamBuilderAttribute);

            var sut = TestFixture.CreateSut();
            sut.Register(TestFixture.TestAssembly);

            var result = new bool[] { sut.IsTypeRegistered<AbstractConstrainedInputObject>(),
                sut.IsTypeRegistered<ConstrainedInputObject>()};

            result.Should()
                .OnlyContain(a => a, "Both types should be found");
        }

        [Fact]
        public void IsTypeRegistered_RegisterNotCalled_ThrowsBuilderRegistrationNotRegisteredException()
        {
            var sut = TestFixture.CreateSut();
            Action act = () => sut.IsTypeRegistered<ConstrainedInputObject>();

            act.ShouldThrow<BuilderRegistrationNotRegisteredException>();
        }

        [Fact]
        public void Build_SingleDefaultBuilder_DefaultObject()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute();

            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            TestFixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = TestFixture.CreateSut();
            sut.Register(TestFixture.TestAssembly);

            var result = sut.Build<ConstrainedInputObject>();

            result.Should()
                .NotBeNull();
        }

        [Fact]
        public void Build_SingleOverridenBuilder_DefaultObectAsBaseType()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var abstractConstrainedInputObjectType = typeof(AbstractConstrainedInputObject);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute(abstractConstrainedInputObjectType);

            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            TestFixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = TestFixture.CreateSut();
            sut.Register(TestFixture.TestAssembly);

            var result = sut.Build<AbstractConstrainedInputObject>();

            result.Should()
                .NotBeNull("Because the builder built an object")
                .And.BeAssignableTo<ConstrainedInputObject>("Because it is actually this type");
        }

        [Fact]
        public void Build_TypeNotRegistered_ThrowsUnregisteredBuilderTypeException()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var typeWithUndefinedBuilder = typeof(DerivedConstrainedInputObject);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute();

            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            TestFixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = TestFixture.CreateSut();
            sut.Register(TestFixture.TestAssembly);

            Action act = () => sut.Build<DerivedConstrainedInputObject>();

            act.ShouldThrow<UnregisteredBuilderTypeException>()
                .Where(a => a.TypeAttemptedToBuild == typeWithUndefinedBuilder);
        }

        [Fact]
        public void Build_RegisterNotCalled_ThrowsBuilderRegistrationNotRegisteredException()
        {
            var sut = TestFixture.CreateSut();
            Action act = () => sut.Build<ConstrainedInputObject>();

            act.ShouldThrow<BuilderRegistrationNotRegisteredException>();
        }

        [Fact]
        public void TryBuild_SingleDefaultBuilder_TrueWithDefaultObject()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute();

            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            TestFixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = TestFixture.CreateSut();
            sut.Register(TestFixture.TestAssembly);

            ConstrainedInputObject objBuilt = null;
            var result = sut.TryBuild(out objBuilt);

            result.Should()
                .BeTrue();
            objBuilt.Should()
                .NotBeNull();
        }

        [Fact]
        public void TryBuild_SingleOverridenBuilder_TrueWithDefaultObectAsBaseType()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var abstractConstrainedInputObjectType = typeof(AbstractConstrainedInputObject);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute(abstractConstrainedInputObjectType);

            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            TestFixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = TestFixture.CreateSut();
            sut.Register(TestFixture.TestAssembly);

            AbstractConstrainedInputObject objBuilt = null;
            var result = sut.TryBuild(out objBuilt);

            result.Should()
                .BeTrue();
            objBuilt.Should()
                .NotBeNull("Because the builder built an object")
                .And.BeAssignableTo<ConstrainedInputObject>("Because it is actually this type");
        }

        [Fact]
        public void TryBuild_TypeNotRegistered_False()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var typeWithUndefinedBuilder = typeof(DerivedConstrainedInputObject);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute();

            TestFixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(TestFixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            TestFixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = TestFixture.CreateSut();
            sut.Register(TestFixture.TestAssembly);

            DerivedConstrainedInputObject objBuilt = null;
            var result = sut.TryBuild(out objBuilt);

            result.Should()
                .BeFalse();
            objBuilt.Should()
                .BeNull();
        }

        [Fact]
        public void TryBuild_RegisterNotCalled_ThrowsBuilderRegistrationNotRegisteredException()
        {
            var sut = TestFixture.CreateSut();
            ConstrainedInputObject objBuilt = null;

            Action act = () => sut.TryBuild(out objBuilt);

            act.ShouldThrow<BuilderRegistrationNotRegisteredException>();
        }
    }
}
