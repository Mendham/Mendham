using Mendham.Testing.Moq.Test.TestObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Moq.Test
{
    public class OrderAgnosticMatchTest
    {
        [Theory, MendhamData]
        public void OrderAgnosticMatch_SameList_IsCalled([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet;

            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithEnumerableParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithEnumerableParameter(Items.OrderAgnosticMatch(secondSet)), Times.Once);
        }

        [Theory, MendhamData]
        public void OrderAgnosticMatch_TwoIdenetialIntSets_IsCalled([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet.ToList();

            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithEnumerableParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithEnumerableParameter(Items.OrderAgnosticMatch(secondSet)), Times.Once);
        }

        [Theory, MendhamData]
        public void OrderAgnosticMatch_SameValuesDifferentOrder_IsCalled([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet
                .OrderBy(a => Guid.NewGuid())
                .ToList();

            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithEnumerableParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithEnumerableParameter(Items.OrderAgnosticMatch(secondSet)), Times.Once);
        }

        [Theory, MendhamData]
        public void OrderAgnosticMatch_SameLengthDifferentValues_IsNotCalled([WithCount(20)]List<int> firstSet,
            [WithCount(20)]List<int> secondSet)
        {
            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithEnumerableParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithEnumerableParameter(Items.OrderAgnosticMatch(secondSet)), Times.Never);
        }

        [Theory, MendhamData]
        public void OrderAgnosticMatch_DifferentLengthDifferentValues_IsNotCalled([WithCount(20)]List<int> firstSet,
            [WithCount(19)]List<int> secondSet)
        {
            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithEnumerableParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithEnumerableParameter(Items.OrderAgnosticMatch(secondSet)), Times.Never);
        }

        [Theory, MendhamData]
        public void OrderAgnosticMatch_ListSameValuesDifferentOrder_IsCalled([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet
                .OrderBy(a => Guid.NewGuid())
                .ToList();

            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithListParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithListParameter(Items.OrderAgnosticMatch<List<int>, int>(secondSet)), Times.Once);
        }

        [Theory, MendhamData]
        public void OrderAgnosticMatch_ArraySameValuesDifferentOrder_IsCalled([WithCount(20)]int[] firstSet)
        {
            var secondSet = firstSet
                .OrderBy(a => Guid.NewGuid())
                .ToArray();

            var sut = Mock.Of<IMethodWithEnumerableParameter>();

            sut.MethodWithArrayParameter(firstSet);

            sut.AsMock()
                .Verify(a => a.MethodWithArrayParameter(Items.OrderAgnosticMatch<int[], int>(secondSet)), Times.Once);
        }
    }
}
