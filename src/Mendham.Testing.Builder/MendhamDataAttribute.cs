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
            var objCreationCtx = ObjectCreationContextFactory.Create(methodAssembly);

            var parameters = testMethod.GetParameters()
                .Select(a => CreateObject(a, objCreationCtx))
                .ToArray();

            return parameters.AsSingleItemEnumerable();
        }

        private static object CreateObject(ParameterInfo parameterInfo, IObjectCreationContext objCreationCtx)
        {
            var createWithCountAttribute = parameterInfo.GetCustomAttribute<CreateWithCountAttribute>();

            if (createWithCountAttribute != default(CreateWithCountAttribute))
            {
                return createWithCountAttribute.CreateObject(parameterInfo, objCreationCtx);
            }
            else
            {
                return objCreationCtx.Create(parameterInfo);
            }
        }
    }
}
