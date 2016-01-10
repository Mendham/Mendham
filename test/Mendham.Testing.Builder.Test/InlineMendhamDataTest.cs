using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test
{
    public class InlineMendhamDataTest
    {
        [Theory]
        [InlineMendhamData("test", 3)]
        public void InlineMendhamData_TwoValuesSet_FromAttribute(string stringFromAttribute, int intFromAttribute)
        {
            stringFromAttribute.Should()
                .Be("test");
            intFromAttribute.Should()
                .Be(3);
        }

        [Theory]
        [InlineMendhamData()]
        public void InlineMendhamData_NoValuesSet_FromMendhamData(string stringFromMendhamData, int intFromMendhamData)
        {
            stringFromMendhamData.Should()
                .NotBeNullOrWhiteSpace();
            intFromMendhamData.Should()
                .NotBe(default(int));
        }

        [Theory]
        [InlineMendhamData("test", 3)]
        public void InlineMendhamData_ValuesFromBoth_FromAttribute(string stringFromAttribute, int intFromAttribute,
            string stringFromMendhamData, int intFromMendhamData)
        {
            stringFromAttribute.Should()
                .Be("test");
            intFromAttribute.Should()
                .Be(3);
            stringFromMendhamData.Should()
                .NotBeNullOrWhiteSpace();
            intFromMendhamData.Should()
                .NotBe(default(int));
        }

        [Theory]
        [InlineMendhamData(4)]
        public void MendhamData_StringParameter_SeededWithVariableName(int fixedVal, string variable1)
        {
            variable1.Should()
                .StartWith(nameof(variable1));
        }
    }
}
