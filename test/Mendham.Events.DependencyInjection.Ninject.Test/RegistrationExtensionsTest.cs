using FluentAssertions;
using Mendham.Events.DependencyInjection.TestObjects;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Events.DependencyInjection.Ninject.Test
{
    public class RegistrationExtensionsTest : IDisposable
    {
        private IKernel sut;

        public RegistrationExtensionsTest()
        {
            sut = new StandardKernel(new EventHandlingModule());
        }

        public void Dispose()
        {
            sut.Dispose();
        }

        [Fact]
        public void RegisterDomainEventHandlers_HandlersInAssembly_ReturnsAll()
        {
            var assembly = typeof(Test1EventHandler).GetTypeInfo().Assembly;

            sut.RegisterEventHandlers(assembly);

            var result = sut.GetAll<IEventHandler>();

            result.Should()
                .NotBeEmpty();
            result.Should()
                .ContainItemsAssignableTo<IEventHandler>();
            result.Should()
                .Contain(a => a is Test1EventHandler);
            result.Should()
                .Contain(a => a is Test2EventHandler);
        }
    }
}
