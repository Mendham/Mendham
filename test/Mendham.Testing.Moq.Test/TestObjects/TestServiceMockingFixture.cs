using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq.Test.TestObjects
{
    public class TestServiceMockingFixture : MockingFixture<TestService>
    {
        public IDependency1 Dependency1 { get; set; }

        public IDependency2 Dependency2 { get; set; }
        
        // Does not have attribute
        [IgnoreFixtureComponent]
        public INonDependency IgnoredDependency { get; set; } 

        public INonDependency DependencyNoSet
        {
            get { throw new InvalidOperationException(); }
        }

        public INonDependency DependencyNoGet
        {
            set { throw new InvalidOperationException(); }
        }

        public INonDependency DependencyPrivateSet { get; private set; }
        public INonDependency DependencyPrivateGet { private get; set; }

        public override TestService CreateSut()
        {
            return new TestService(Dependency1, Dependency2);
        }
    }
}
