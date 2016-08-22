using Autofac;
using FluentAssertions;
using Mendham.Events;
using Mendham.Events.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Autofac.Test
{
	public class EventHandlingModuleTest
	{
		[Fact]
		public void DomainEventHandlingModule_RegisterEventPublisher_Resolves()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<EventHandlingModule>();

			using (var sut = builder.Build().BeginLifetimeScope())
			{
				var publisher = sut.Resolve<IEventPublisher>();

				publisher.Should()
					.NotBeNull()
					.And.BeOfType<EventPublisher>();
			}
		}

		[Fact]
		public void DomainEventHandlingModule_RegisterEventHandlingContainer_Resolves()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<EventHandlingModule>();

			using (var sut = builder.Build().BeginLifetimeScope())
			{
				var publisher = sut.Resolve<IEventHandlerContainer>();

				publisher.Should()
					.NotBeNull()
					.And.BeOfType<DefaultEventHandlerContainer>();
			}
		}

        [Fact]
        public void DomainEventHandlingModule_RegisterEventHandlingProcessor_Resolves()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var publisher = sut.Resolve<IEventHandlerProcessor>();

                publisher.Should()
                    .NotBeNull()
                    .And.BeOfType<EventHandlerProcessor>();
            }
        }

        [Fact]
        public void DomainEventHandlingModule_RegisterEventLoggerProcessor_Resolves()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var publisher = sut.Resolve<IEventLoggerProcessor>();

                publisher.Should()
                    .NotBeNull()
                    .And.BeOfType<EventLoggerProcessor>();
            }
        }

        [Fact]
		public void DomainEventHandlingModule_RegisterEventPublisher_IsSameInstance()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<EventHandlingModule>();
			var container = builder.Build();

			IEventPublisher publisher1, publisher2;

			using (var sut =container.BeginLifetimeScope())
			{
				publisher1 = sut.Resolve<IEventPublisher>();
			}

			using (var sut =container.BeginLifetimeScope())
			{
				publisher2 = sut.Resolve<IEventPublisher>();
			}

			publisher1.Should()
				.BeSameAs(publisher2);
		}

		[Fact]
		public void DomainEventHandlingModule_RegisterEventHandlingContainer_IsSameInstance()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<EventHandlingModule>();
			var container = builder.Build();

			IEventHandlerContainer container1, container2;

			using (var sut =container.BeginLifetimeScope())
			{
				container1 = sut.Resolve<IEventHandlerContainer>();
			}

			using (var sut =container.BeginLifetimeScope())
			{
				container2 = sut.Resolve<IEventHandlerContainer>();
			}

			container1.Should()
				.BeSameAs(container2);
		}

        [Fact]
        public void DomainEventHandlingModule_RegisterDomainEventHandlingProcessor_IsSameInstance()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();
            var container = builder.Build();

            IEventHandlerProcessor processor1, processor2;

            using (var sut = container.BeginLifetimeScope())
            {
                processor1 = sut.Resolve<IEventHandlerProcessor>();
            }

            using (var sut = container.BeginLifetimeScope())
            {
                processor2 = sut.Resolve<IEventHandlerProcessor>();
            }

            processor1.Should()
                .BeSameAs(processor2);
        }

        [Fact]
        public void DomainEventHandlingModule_RegisterEventLoggerProcessor_IsSameInstance()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<EventHandlingModule>();
            var container = builder.Build();

            IEventLoggerProcessor loggerProcessor1, loggerProcessor2;

            using (var sut = container.BeginLifetimeScope())
            {
                loggerProcessor1 = sut.Resolve<IEventLoggerProcessor>();
            }

            using (var sut = container.BeginLifetimeScope())
            {
                loggerProcessor2 = sut.Resolve<IEventLoggerProcessor>();
            }

            loggerProcessor1.Should()
                .BeSameAs(loggerProcessor2);
        }
    }
}
