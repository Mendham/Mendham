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
            var objCreationCtx = ObjectCreationContextFactory.Create(methodAssembly) as IFullObjectCreationContext;

            var parameters = testMethod.GetParameters()
                .Select(a => CreateObject(a, objCreationCtx))
                .ToArray();

            return parameters.AsSingleItemEnumerable();
        }

        private static object CreateObject(ParameterInfo parameterInfo, IFullObjectCreationContext objCreationCtx)
        {
            var withCountAttribute = parameterInfo.GetCustomAttribute<WithCountAttribute>();

            if (withCountAttribute != default(WithCountAttribute))
            {
                return withCountAttribute.CreateObject(parameterInfo, objCreationCtx);
            }
            else
            {
                return objCreationCtx.Create(parameterInfo);
            }
        }
    }
}
