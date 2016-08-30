using System;

namespace Mendham.Testing.Moq.Test.TestObjects
{
    public class NonCreationMockingFixture : MockingFixture
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
    }
}
