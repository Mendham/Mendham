using Mendham.Testing.Moq.Test.TestObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Moq.Test
{
    public class OrderAgnosticTest
    {
        // Only running these tests in NET451 because Mendham.Testing.Builder does not work with Netstandard because of the
        // underlying dependencies. When those depdencies are upgraded to netstandard, then the if condition can be removed.
#if NET451

        [Theory, MendhamData]
        public void Match_SameList_IsCalled([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet;

            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithEnumerableParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithEnumerableParameter(OrderAgnostic.Match(secondSet)), Times.Once);
        }

        [Theory, MendhamData]
        public void Match_TwoIdenetialIntSets_IsCalled([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet.ToList();

            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithEnumerableParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithEnumerableParameter(OrderAgnostic.Match(secondSet)), Times.Once);
        }

        [Theory, MendhamData]
        public void Match_SameValuesDifferentOrder_IsCalled([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet
                .OrderBy(a => Guid.NewGuid())
                .ToList();

            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithEnumerableParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithEnumerableParameter(OrderAgnostic.Match(secondSet)), Times.Once);
        }

        [Theory, MendhamData]
        public void Match_SameLengthDifferentValues_IsNotCalled([WithCount(20)]List<int> firstSet,
            [WithCount(20)]List<int> secondSet)
        {
            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithEnumerableParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithEnumerableParameter(OrderAgnostic.Match(secondSet)), Times.Never);
        }

        [Theory, MendhamData]
        public void Match_DifferentLengthDifferentValues_IsNotCalled([WithCount(20)]List<int> firstSet,
            [WithCount(19)]List<int> secondSet)
        {
            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithEnumerableParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithEnumerableParameter(OrderAgnostic.Match(secondSet)), Times.Never);
        }

        [Theory, MendhamData]
        public void Match_ListSameValuesDifferentOrder_IsCalled([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet
                .OrderBy(a => Guid.NewGuid())
                .ToList();

            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithListParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithListParameter(OrderAgnostic.Match<List<int>, int>(secondSet)), Times.Once);
        }

        [Theory, MendhamData]
        public void Match_ArraySameValuesDifferentOrder_IsCalled([WithCount(20)]int[] firstSet)
        {
            var secondSet = firstSet
                .OrderBy(a => Guid.NewGuid())
                .ToArray();

            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithArrayParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithArrayParameter(OrderAgnostic.Match<int[], int>(secondSet)), Times.Once);
        }
#endif
    }
}
