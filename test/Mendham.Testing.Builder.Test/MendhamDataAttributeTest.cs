using FluentAssertions;
using Mendham.Testing.Builder.Test.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test1
{
    public class MendhamDataAttributeTest
    {
        [Theory]
        [MendhamData]
        public void MendhamData_ActualType_CreatesConstrainedInputObject(ConstrainedInputObject obj)
        {
            var sut = obj;

            sut.Should()
                .NotBeNull();
            sut.Value.Should()
                .HaveLength(3);
        }

        [Theory]
        [MendhamData]
        public void MendhamData_AbstractType_CreatesConstrainedInputObject(AbstractConstrainedInputObject obj)
        {
            var sut = obj;

            sut.Should()
                .NotBeNull();
            sut.Value.Should()
                .HaveLength(3);
        }

        [Theory]
        [MendhamData]
        public void MendhamData_DerivedType_CreatesDerivedConstrainedInputObject(DerivedConstrainedInputObject obj)
        {
            var sut = obj;

            sut.Should()
                .NotBeNull();
            sut.Value.Should()
                .HaveLength(3);
            sut.DerivedValue.Should()
                .NotBe(default(int));
        }

        [Theory]
        [MendhamData]
        public void MendhamData_String_NotNullOrEmpty(string obj)
        {
            var sut = obj;

            sut.Should()
                .NotBeNullOrEmpty();
        }

        [Theory]
        [MendhamData]
        public void MendhamData_TwoStrings_NotEqual(string obj1, string obj2)
        {
            var sut = obj1;

            sut.Should()
                .NotBe(obj2);
        }

        [Theory]
        [MendhamData]
        public void MendhamData_StringParameter_SeededWithVariableName(string variable1)
        {
            variable1.Should()
                .StartWith(nameof(variable1));
        }
    }
}
