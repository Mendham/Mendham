using FluentAssertions;
using Mendham.Testing.Builder.Test.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test
{
    public class SharedObjectCreationContextTest
    {
        // The purpose of this test is to verify that that the values from MendhamData
        // and the values created by builders share the same context and will not have
        // numbers that repeat when the builder and test are in the same class
        [Theory, MendhamData]
        public void SharedContext_ParamterAndInMethod_NoSharedValues
            ([CreateWithCount(250)]IEnumerable<int> setFromParameter)
        {
            var builderObjectWithSet = new ObjectWithCollectionBuilder()
                .WithItemCount(250)
                .Build();
            var setFromBuilderObject = builderObjectWithSet.Collection;

            var shareAValue = setFromParameter
                .Any(a => setFromBuilderObject.Contains(a));

            shareAValue.Should()
                .BeFalse("because the fixture is shared and prevents items from being repeated");
        }
    }
}
