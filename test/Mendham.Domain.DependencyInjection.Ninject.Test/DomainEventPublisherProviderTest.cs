using FluentAssertions;
using Mendham.Domain.Events;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Ninject.Test
{
    public class DomainEventPublisherProviderTest : IDisposable
    {
        private IKernel sut;

        public DomainEventPublisherProviderTest()
        {
            this.sut = new StandardKernel(new DomainEventHandlingModule());
        }

        public void Dispose()
        {
            this.sut.Dispose();
        }

        [Fact]
        public void GetPublisher_SingleScope_ValidPublisher()
        {

            var provider = sut.Get<IDomainEventPublisherProvider>();
            var publisher = provider.GetPublisher();

            publisher.Should()
                .NotBeNull();
        }

        [Fact]
        public void GetPublisher_CalledTwice_ReturnsEquivalentPublishers()
        {

            var provider = sut.Get<IDomainEventPublisherProvider>();
            var publisher1 = provider.GetPublisher();
            var publisher2 = provider.GetPublisher();

            publisher1
                .ShouldBeEquivalentTo(publisher2);
        }
    }
}
