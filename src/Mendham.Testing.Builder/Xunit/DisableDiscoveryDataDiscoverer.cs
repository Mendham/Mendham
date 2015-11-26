using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Mendham.Testing.Xunit
{
    public class DisableDiscoveryDataDiscoverer : DataDiscoverer
    {
        public override bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod)
        {
            // Values that come out of MendhamDataAttribute are not stable and therefore discovery
            // is not supported. If this is not applied, tests will not run.
            return false;
        }
    }
}
