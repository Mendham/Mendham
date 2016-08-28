using FluentAssertions;
using Mendham.Domain.DependencyInjection.InvalidConcreateBaseEntity;
using Mendham.Domain.DependencyInjection.InvalidMultipleDerivedEntity;
using Mendham.Domain.DependencyInjection.TestObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.AspNetCore.Test
{
    public class DomainRegistrationExtensionsTest
    {
        [Fact]
        public void AddDomainFacades_ApplyingToBuilder_ReturnsFacade()
        {
            var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;

            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddDomainFacades(assembly))
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var result = server.Host.Services.GetService<TestEntityWithDomainFacade.IFacade>();

                result.Should().NotBeNull();
            }
        }

        [Fact]
        public void AddDomainFacades_DerivedInterface_ReturnsDerivedFacade()
        {
            var assembly = typeof(DerivedTestEntityWithDomainFacade).GetTypeInfo().Assembly;

            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddDomainFacades(assembly))
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var result = server.Host.Services.GetService<DerivedTestEntityWithDomainFacade.IDerivedFacade>();

                result.Should().NotBeNull();
            }
        }

        [Fact]
        public void AddDomainFacades_BaseInterfaceOnAbstractBase_ReturnsNonAbstractFacade()
        {
            var assembly = typeof(AbstractTestEntityWithDomainFacade).GetTypeInfo().Assembly;

            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddDomainFacades(assembly))
                .Configure(app => { });
            
            using (var server = new TestServer(builder))
            {
                var facade = server.Host.Services.GetService<AbstractTestEntityWithDomainFacade.IBaseFacade>();

                facade.Should()
                    .NotBeNull("there is a non abstract facade assocaited with IBaseFacade")
                    .And.BeOfType<DerivedTestEntityWithDomainFacade.DerivedFacade>("this is the one and only non abstract class");
            }
        }

        [Fact]
        public void AddDomainFacades_UnrelatedInterface_NotBound()
        {
            var assembly = typeof(IUnrelatedInterface).GetTypeInfo().Assembly;

            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddDomainFacades(assembly))
                .Configure(app => { });
            
            using (var server = new TestServer(builder))
            {
                var result = server.Host.Services.GetService<IUnrelatedInterface>();

                result.Should().BeNull();
            }
        }

        [Fact]
        public void AddDomainFacades_InvalidConditionSharedFacadeBetweenBaseAndDerivedEntity_ThrowsMultipleDomainFacadesFoundException()
        {
            var assembly = typeof(ConcreateBaseEntity).GetTypeInfo().Assembly;

            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddDomainFacades(assembly))
                .Configure(app => { });

            Action act = () => new TestServer(builder);

            act.ShouldThrow<MultipleDomainFacadesFoundException>()
                .Where(a => a.InterfaceToBind.Equals(typeof(ConcreateBaseEntity.IBaseFacade)), "IBaseFacade is implemented in two concreate classes")
                .Where(a => a.TypesImplementingInterface.Count() == 2, "There are two classes that implement the base facade")
                .Where(a => a.TypesImplementingInterface.Contains(typeof(ConcreateBaseEntity.BaseFacade)), "BaseFacade is concreate and implements IBaseFacade")
                .Where(a => a.TypesImplementingInterface.Contains(typeof(DerivedFromConcreateBaseEntity.DerivedFacade)), "DerivedFacade is concreate and implements IBaseFacade");
        }

        [Fact]
        public void AddDomainFacades_InvalidConditionSharedFacadeMultipleDerivedEntity_ThrowsMultipleDomainFacadesFoundException()
        {
            var assembly = typeof(AbstractBaseEntity).GetTypeInfo().Assembly;

            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddDomainFacades(assembly))
                .Configure(app => { });
            
            Action act = () => builder.Build();

            act.ShouldThrow<MultipleDomainFacadesFoundException>()
                .Where(a => a.InterfaceToBind.Equals(typeof(AbstractBaseEntity.IBaseFacade)), "IBaseFacade is implemented in two concreate classes")
                .Where(a => a.TypesImplementingInterface.Count() == 2, "There are two classes that implement the base facade")
                .Where(a => a.TypesImplementingInterface.Contains(typeof(DerivedFromAbstractBaseEntity.DerivedFacade)), "DerivedFacade in DerivedFromAbstractBaseEntity implements IBaseFacade")
                .Where(a => a.TypesImplementingInterface.Contains(typeof(AltDerivedFromAbstractBaseEntity.DerivedFacade)), "DerivedFacade in DerivedFromAbstractBaseEntity implements IBaseFacade");
        }

        [Fact]
        public void AddDomainFacades_IgnoreConcreateBaseSetManually_ReturnsBaseFacade()
        {
            var assembly = typeof(ConcreateBaseEntity).GetTypeInfo().Assembly;
            var interfacesToIgnore = typeof(ConcreateBaseEntity.IBaseFacade).AsSingleItemEnumerable();

            var builder = new WebHostBuilder()
                .ConfigureServices(sc =>
                {
                    sc.AddDomainFacades(assembly, interfacesToIgnore);
                    sc.AddTransient<ConcreateBaseEntity.IBaseFacade, ConcreateBaseEntity.BaseFacade>();
                })
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var result = server.Host.Services.GetService<ConcreateBaseEntity.IBaseFacade>();

                result.Should().NotBeNull();
            }
        }

        [Fact]
        public void AddDomainFacades_IgnoreConcreateBaseNotSettingBase_BaseFacadeNotSet()
        {
            var assembly = typeof(ConcreateBaseEntity).GetTypeInfo().Assembly;
            var interfacesToIgnore = typeof(ConcreateBaseEntity.IBaseFacade).AsSingleItemEnumerable();

            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddDomainFacades(assembly, interfacesToIgnore))
                .Configure(app => { });
            
            using (var server = new TestServer(builder))
            {
                var result = server.Host.Services.GetService<ConcreateBaseEntity.IBaseFacade>();

                result.Should().BeNull("the base facade is not set");
            }
        }

        [Fact]
        public void AddDomainFacades_IgnoreConcreateBase_ReturnsDerivedFacade()
        {
            var assembly = typeof(ConcreateBaseEntity).GetTypeInfo().Assembly;
            var interfacesToIgnore = typeof(ConcreateBaseEntity.IBaseFacade).AsSingleItemEnumerable();

            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddDomainFacades(assembly, interfacesToIgnore))
                .Configure(app => { });
            
            using (var server = new TestServer(builder))
            {
                var result = server.Host.Services.GetService<DerivedFromConcreateBaseEntity.IDerivedFacade>();

                result.Should().NotBeNull();
            }
        }

        [Fact]
        public void AddDomainFacades_IgnoreListIncludesNonInterface_ThrowsInvalidDomainFacadeExclusionException()
        {
            var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;
            var nonInterfaceType = typeof(TestEntityWithDomainFacade.Facade);
            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddDomainFacades(assembly, nonInterfaceType.AsSingleItemEnumerable()))
                .Configure(app => { });

            Action act = () => new TestServer(builder);

            act.ShouldThrow<InvalidDomainFacadeExclusionException>()
                .Where(a => a.InvalidType.Equals(nonInterfaceType), "it is not an interface");
        }

        [Fact]
        public void AddDomainFacades_IgnoreListIncludesInterfaceNotDerivedFromDomainFacade_ThrowsInvalidDomainFacadeExclusionException()
        {
            var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;
            var interfaceNotDerivedFromDomainFacade = typeof(IUnrelatedInterface);
            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddDomainFacades(assembly, interfaceNotDerivedFromDomainFacade.AsSingleItemEnumerable()))
                .Configure(app => { });

            Action act = () => new TestServer(builder);

            act.ShouldThrow<InvalidDomainFacadeExclusionException>()
                .Where(a => a.InvalidType.Equals(interfaceNotDerivedFromDomainFacade), "the interface is not derived from IDomainFacade");
        }

        [Fact]
        public void AddDomain_ApplyingToBuilder_ReturnsEntity()
        {
            var assembly = typeof(TestEntityWithDomainFacade).GetTypeInfo().Assembly;

            var builder = new WebHostBuilder()
                .ConfigureServices(sc =>
                {
                    sc.AddDomain(assembly);
                    sc.AddTransient<TestEntityWithDomainFacade.Factory>(sp =>
                        val => new TestEntityWithDomainFacade(val, sp.GetService<TestEntityWithDomainFacade.IFacade>()));
                })
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var factory = server.Host.Services.GetService<TestEntityWithDomainFacade.Factory>();

                var entity = factory(7);

                entity.Should().NotBeNull();
                entity.HasValidFacade().Should().BeTrue();
            }
        }
    }
}
