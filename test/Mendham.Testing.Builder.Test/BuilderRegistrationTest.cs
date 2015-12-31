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
    public class BuilderRegistrationTest : UnitTest<BuilderRegistrationFixture>
    {
        public BuilderRegistrationTest(BuilderRegistrationFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Register_BuilderTypeDoesNotImplementIBuilder_ThrowsInvalidBuilderException()
        {
            var classThatDoesNotImplementIBuilder = typeof(ClassThatDoesNotImplementIBuilder);
            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(classThatDoesNotImplementIBuilder);

            var sut = Fixture.CreateSut();
            Action act = () => sut.Register(Fixture.TestAssembly);

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

            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(builderType);
            Fixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderType))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = Fixture.CreateSut();
            Action act = () => sut.Register(Fixture.TestAssembly);

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

            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(firstBuilder, secondBuilder);
            Fixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(It.IsAny<Type>()))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = Fixture.CreateSut();
            Action act = () => sut.Register(Fixture.TestAssembly);

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

            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            Fixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = Fixture.CreateSut();
            sut.Register(Fixture.TestAssembly);

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

            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            Fixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = Fixture.CreateSut();
            sut.Register(Fixture.TestAssembly);

            var result = sut.IsTypeRegistered<AbstractConstrainedInputObject>();

            result.Should()
                .BeTrue();
        }

        [Fact]
        public void IsTypeRegistered_TypeNotRegistered_False()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute();

            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            Fixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = Fixture.CreateSut();
            sut.Register(Fixture.TestAssembly);

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

            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            Fixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(defaultMendhamBuilderAttribute, overridingMendhamBuilderAttribute);

            var sut = Fixture.CreateSut();
            sut.Register(Fixture.TestAssembly);

            var result = new bool[] { sut.IsTypeRegistered<AbstractConstrainedInputObject>(),
                sut.IsTypeRegistered<ConstrainedInputObject>()};

            result.Should()
                .OnlyContain(a => a, "Both types should be found");
        }

        [Fact]
        public void IsTypeRegistered_RegisterNotCalled_ThrowsBuilderRegistrationNotRegisteredException()
        {
            var sut = Fixture.CreateSut();
            Action act = () => sut.IsTypeRegistered<ConstrainedInputObject>();

            act.ShouldThrow<BuilderRegistrationNotRegisteredException>();
        }

        [Fact]
        public void BuildGeneric_SingleDefaultBuilder_DefaultObject()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute();

            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            Fixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = Fixture.CreateSut();
            sut.Register(Fixture.TestAssembly);

            var result = sut.Build<ConstrainedInputObject>();

            result.Should()
                .NotBeNull();
        }

        [Fact]
        public void BuildGeneric_SingleOverridenBuilder_DefaultObjectAsBaseType()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var abstractConstrainedInputObjectType = typeof(AbstractConstrainedInputObject);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute(abstractConstrainedInputObjectType);

            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            Fixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = Fixture.CreateSut();
            sut.Register(Fixture.TestAssembly);

            var result = sut.Build<AbstractConstrainedInputObject>();

            result.Should()
                .NotBeNull("Because the builder built an object")
                .And.BeAssignableTo<ConstrainedInputObject>("Because it is actually this type");
        }

        [Fact]
        public void BuildGeneric_TypeNotRegistered_ThrowsUnregisteredBuilderTypeException()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var typeWithUndefinedBuilder = typeof(DerivedConstrainedInputObject);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute();

            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            Fixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = Fixture.CreateSut();
            sut.Register(Fixture.TestAssembly);

            Action act = () => sut.Build<DerivedConstrainedInputObject>();

            act.ShouldThrow<UnregisteredBuilderTypeException>()
                .Where(a => a.TypeAttemptedToBuild == typeWithUndefinedBuilder);
        }

        [Fact]
        public void BuildGeneric_RegisterNotCalled_ThrowsBuilderRegistrationNotRegisteredException()
        {
            var sut = Fixture.CreateSut();
            Action act = () => sut.Build<ConstrainedInputObject>();

            act.ShouldThrow<BuilderRegistrationNotRegisteredException>();
        }

        [Fact]
        public void Build_SingleDefaultBuilder_DefaultObject()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute();

            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            Fixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = Fixture.CreateSut();
            sut.Register(Fixture.TestAssembly);

            var result = sut.Build(typeof(ConstrainedInputObject));

            result.Should()
                .NotBeNull();
        }

        [Fact]
        public void Build_SingleOverridenBuilder_DefaultObjectAsBaseType()
        {
            var builderTypeForConstrainedInputObject = typeof(ConstrainedInputObjectBuilder);
            var abstractConstrainedInputObjectType = typeof(AbstractConstrainedInputObject);
            var mendhamBuilderAttribute = new MendhamBuilderAttribute(abstractConstrainedInputObjectType);

            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            Fixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = Fixture.CreateSut();
            sut.Register(Fixture.TestAssembly);

            var result = sut.Build(typeof(AbstractConstrainedInputObject));

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

            Fixture.BuilderQueryService.AsMock()
                .Setup(a => a.GetBuilderTypes(Fixture.TestAssembly))
                .ReturnItems(builderTypeForConstrainedInputObject);
            Fixture.BuilderAttributeResolver.AsMock()
                .Setup(a => a.GetAttributesAppliedToBuilder(builderTypeForConstrainedInputObject))
                .ReturnItems(mendhamBuilderAttribute);

            var sut = Fixture.CreateSut();
            sut.Register(Fixture.TestAssembly);

            Action act = () => sut.Build(typeof(DerivedConstrainedInputObject));

            act.ShouldThrow<UnregisteredBuilderTypeException>()
                .Where(a => a.TypeAttemptedToBuild == typeWithUndefinedBuilder);
        }

        [Fact]
        public void Build_RegisterNotCalled_ThrowsBuilderRegistrationNotRegisteredException()
        {
            var sut = Fixture.CreateSut();
            Action act = () => sut.Build(typeof(ConstrainedInputObject));

            act.ShouldThrow<BuilderRegistrationNotRegisteredException>();
        }
    }
}
