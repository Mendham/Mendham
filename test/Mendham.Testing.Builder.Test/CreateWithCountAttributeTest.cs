using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test
{
    public class CreateWithCountAttributeTest
    {
        [Theory, MendhamData]
        public void CreateWithCount_Ten_TenGuids([CreateWithCount(10)]IEnumerable<Guid> values)
        {
            values.Should()
                .HaveCount(10, "that is the size set in the CreateByCountAttribute");
        }
    }
}
