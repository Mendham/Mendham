using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test
{
    public class BuilderAssemblyQueryServiceTest
    {
        [Fact]
        public void GetAssembliesWithBuilders_ThisAssembly_ThisAssembly()
        {
            var thisAssembly = GetType().Assembly;
            var sut = new BuilderAssemblyQueryService();

            var result = sut.GetAssembliesWithBuilders(thisAssembly);

            result.Should()
                .HaveCount(1)
                .And.Contain(thisAssembly);
        }
    }
}
