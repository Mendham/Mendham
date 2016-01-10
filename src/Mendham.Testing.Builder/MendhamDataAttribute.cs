using Mendham;
using Mendham.Testing.Builder;
using Mendham.Testing.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Mendham.Testing
{
    [DataDiscoverer("Mendham.Testing.Xunit.DisableDiscoveryDataDiscoverer",
        "Mendham.Testing.Builder")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MendhamDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            testMethod.VerifyArgumentNotDefaultValue("Test method is required");

            var methodAssembly = testMethod.DeclaringType.Assembly;
            var objCreationCtx = new ObjectCreationContext(methodAssembly);

            var parameters = testMethod.GetParameters()
                .Select(a => objCreationCtx.CreateByParameter(a))
                .ToArray();

            return parameters.AsSingleItemEnumerable();
        }
    }
}
