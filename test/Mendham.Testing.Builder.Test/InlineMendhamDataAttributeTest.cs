using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test
{
    public class InlineMendhamDataAttributeTest
    {
        [Theory]
        [InlineData("test", 3)]
        public void GetData_MethodMixedParameters_EnumerationOfObjectArray(string stringFromAttribute, int intFromAttribute)
        {
            var methodInfo = this.GetType()
                .GetMethod("SampleTestMethod");

            var sut = new InlineMendhamDataAttribute(stringFromAttribute, intFromAttribute);

            var result = sut.GetData(methodInfo);

            result.Should()
                .HaveCount(1);
            result.First().Should()
                .HaveCount(4)
                .And.HaveElementAt(0, stringFromAttribute)
                .And.HaveElementAt(1, intFromAttribute)
                .And.Match(a => !string.IsNullOrWhiteSpace(a.ToList()[2].ToString()))
                .And.Match(a => (int)(a.ToList()[3]) != default(int));
                
        }

        public void SampleTestMethod(string stringFromAttribute, int intFromAttribute,
            string stringFromMendhamData, int intFromMendhamData)
        {
            // This method is only used for its signature
            throw new NotImplementedException("No need to implement this.");
        }
    }
}
