using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq.Test.TestObjects
{
    public class TestServiceMockingFixture : MockingFixture<TestService>
    {
        [FixtureComponent]
        public IDependency1 Dependency1 { get; set; }

        [FixtureComponent]
        public IDependency2 Dependency2 { get; set; }
        
        // Does not have attribute
        public INonDependency NonDependency { get; set; } 


        public override TestService CreateSut()
        {
            return new TestService(Dependency1, Dependency2);
        }
    }
}
