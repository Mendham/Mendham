using FluentAssertions;
using Mendham.Testing.Moq.Test.TestObjects;
using Moq;
using Xunit;

namespace Mendham.Testing.Moq.Test
{
    public class AsMockExtensionsTest
    {
        [Fact]
        public void AsMock_ObjectFromMoq_Mock()
        {
            var sut = Mock.Of<IMockableContract>();

            var result = sut.AsMock();

            result.Should()
                .NotBeNull()
                .And.BeOfType<Mock<IMockableContract>>();
        }
    }
}
