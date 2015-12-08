using FluentAssertions;
using Mendham.Testing.Builder.Test.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test
{
    public class BuilderExtensionTest
    {
        [Theory]
        [MendhamData]
        public void BuildMultiple_CommonValue_MultipleObjectsWithValue(int commonVal, int count)
        {
            var result = new BasicObjectBuilder()
                .WithIntVal(commonVal)
                .BuildMultiple(count);

            result.Should().HaveCount(count, "that was the count passed")
                .And.OnlyContain(a => a.IntVal == commonVal, "they were all set with that value");
        }
    }
}
