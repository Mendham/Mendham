using FluentAssertions;
using Mendham.Testing.Moq.Test.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Moq.Test
{
    public class MockableObjectBuilderExtensionsTest
    {
        [Fact]
        public void SetupMock_ApplyRule_ResultFromRule()
        {
            int expected = 19;
            var sut = new BuildableObjectBuilder()
                .SetupMock()
                .ApplyMockRule(m => m
                    .Setup(a => a.GetValue())
                    .Returns(expected))
                .Build();

            var result = sut.GetValue();

            result.Should()
                .Be(expected);
        }
    }
}
