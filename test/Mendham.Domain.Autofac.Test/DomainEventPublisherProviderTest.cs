using Autofac;
using FluentAssertions;
using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Autofac.Test
{
    public class DomainEventPublisherProviderTest
    {
        [Fact]
        public void GetPublisher_SingleScope_ValidPublisher()
        {
            var builder = new ContainerBuilder();
			builder.RegisterModule<DomainEventHandlingModule>();

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var provider = sut.Resolve<IDomainEventPublisherProvider>();
                var publisher = provider.GetPublisher();

                publisher
                    .Should()
                    .NotBeNull();
            }
        }

        [Fact]
        public void GetPublisher_CalledTwice_ReturnsEquivalentPublishers()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<DomainEventHandlingModule>();

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var provider = sut.Resolve<IDomainEventPublisherProvider>();
                var publisher1 = provider.GetPublisher();
                var publisher2 = provider.GetPublisher();

                publisher1
                    .ShouldBeEquivalentTo(publisher2);
            }
        }
    }
}