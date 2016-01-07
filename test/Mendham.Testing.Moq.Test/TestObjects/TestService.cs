using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq.Test.TestObjects
{
    public class TestService
    {
        private readonly IDependency1 dependency1;
        private readonly IDependency2 dependency2;

        public TestService(IDependency1 dependency1, IDependency2 dependency2)
        {
            this.dependency1 = dependency1;
            this.dependency2 = dependency2;
        }

        public IDependency1 Dependency1
        {
            get
            {
                return dependency1;
            }
        }

        public IDependency2 Dependency2
        {
            get
            {
                return dependency2;
            }
        }
    }
}
