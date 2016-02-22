using Autofac;
using FluentAssertions;
using Mendham.Domain.Events;
using Mendham.Domain.Events.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Autofac.Test
{
	public class DomainEventHandlingModuleTest
	{
		[Fact]
		public void DomainEventHandlingModule_RegisterDomainEventPublisher_Resolves()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<DomainEventHandlingModule>();

			using (var sut = builder.Build().BeginLifetimeScope())
			{
				var publisher = sut.Resolve<IDomainEventPublisher>();

				publisher.Should()
					.NotBeNull()
					.And.BeOfType<DomainEventPublisher>();
			}
		}

		[Fact]
		public void DomainEventHandlingModule_RegisterDomainEventHandlingContainer_Resolves()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<DomainEventHandlingModule>();

			using (var sut = builder.Build().BeginLifetimeScope())
			{
				var publisher = sut.Resolve<IDomainEventHandlerContainer>();

				publisher.Should()
					.NotBeNull()
					.And.BeOfType<DefaultDomainEventHandlerContainer>();
			}
		}

        [Fact]
        public void DomainEventHandlingModule_RegisterDomainEventHandlingProcessor_Resolves()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<DomainEventHandlingModule>();

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var publisher = sut.Resolve<IDomainEventHandlerProcessor>();

                publisher.Should()
                    .NotBeNull()
                    .And.BeOfType<DomainEventHandlerProcessor>();
            }
        }

        [Fact]
		public void DomainEventHandlingModule_RegisterDomainEventPublisher_IsSameInstance()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<DomainEventHandlingModule>();
			var container = builder.Build();

			IDomainEventPublisher publisher1, publisher2;

			using (var sut =container.BeginLifetimeScope())
			{
				publisher1 = sut.Resolve<IDomainEventPublisher>();
			}

			using (var sut =container.BeginLifetimeScope())
			{
				publisher2 = sut.Resolve<IDomainEventPublisher>();
			}

			publisher1.Should()
				.BeSameAs(publisher2);
		}

		[Fact]
		public void DomainEventHandlingModule_RegisterDomainEventHandlingContainer_IsSameInstance()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<DomainEventHandlingModule>();
			var container = builder.Build();

			IDomainEventHandlerContainer container1, container2;

			using (var sut =container.BeginLifetimeScope())
			{
				container1 = sut.Resolve<IDomainEventHandlerContainer>();
			}

			using (var sut =container.BeginLifetimeScope())
			{
				container2 = sut.Resolve<IDomainEventHandlerContainer>();
			}

			container1.Should()
				.BeSameAs(container2);
		}

        [Fact]
        public void DomainEventHandlingModule_RegisterDomainEventHandlingProcessor_IsSameInstance()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<DomainEventHandlingModule>();
            var container = builder.Build();

            IDomainEventHandlerProcessor processor1, processor2;

            using (var sut = container.BeginLifetimeScope())
            {
                processor1 = sut.Resolve<IDomainEventHandlerProcessor>();
            }

            using (var sut = container.BeginLifetimeScope())
            {
                processor2 = sut.Resolve<IDomainEventHandlerProcessor>();
            }

            processor1.Should()
                .BeSameAs(processor2);
        }
    }
}
