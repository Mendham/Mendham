using FluentAssertions;
using Mendham.Testing.Moq.Test.TestObjects;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Moq.Test
{
    public class ReturnExtensionsTest
    {
        [Fact]
        public async Task ReturnItmesAsync_Ints_AllInts()
        {
            var mock = new Mock<IMockableContract>();
            var items = new int[] { 4, 5, 6 };

            mock.Setup(a => a.GetValuesAsync())
                .ReturnsItemsAsync(items);

            var sut = mock.Object;

            var result = await sut.GetValuesAsync();

            result.Should()
                .Equal(items);
        }

        [Fact]
        public void ReturnsInOrder_Ints_IntsInOrder()
        {
            int firstInt = 7;
            int secondInt = 11;
            int thirdInt = 13;

            var mock = new Mock<IMockableContract>();

            mock.Setup(a => a.GetValue())
                .ReturnsInOrder(firstInt, secondInt, thirdInt);

            var sut = mock.Object;

            var result = new List<int>
            {
                sut.GetValue(),
                sut.GetValue(),
                sut.GetValue()
            };

            result.Should()
                .ContainInOrder(firstInt, secondInt, thirdInt);
        }

        [Fact]
        public async Task ReturnsInOrderAsync_Ints_IntsInOrder()
        {
            int firstInt = 7;
            int secondInt = 11;
            int thirdInt = 13;

            var mock = new Mock<IMockableContract>();

            mock.Setup(a => a.GetValueAsync())
                .ReturnsInOrderAsync(firstInt, secondInt, thirdInt);

            var sut = mock.Object;

            var result = await Task.WhenAll(
                sut.GetValueAsync(),
                sut.GetValueAsync(),
                sut.GetValueAsync());

            result.Should()
                .ContainInOrder(firstInt, secondInt, thirdInt);
        }

        [Fact]
        public void ReturnsNoActionTask_Called_EmptyTask()
        {
            var mock = new Mock<IMockableContract>();

            mock.Setup(a => a.DoSomethingAsync())
                .ReturnsNoActionTask();

            var sut = mock.Object;

            var result = sut.DoSomethingAsync();

            result.Should()
                .NotBeNull()
                .And.Subject.As<Task>().IsCompleted.Should()
                .BeTrue();
            mock.Verify(a => a.DoSomethingAsync(), Times.Once);
        }
    }
}
