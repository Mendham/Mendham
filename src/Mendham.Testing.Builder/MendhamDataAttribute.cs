using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Mendham.Testing
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MendhamDataAttribute : Attribute//DataAttribute
    {
        //public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        //{
        //    var methodAssembly = testMethod.DeclaringType.Assembly;

        //    throw new NotImplementedException();
        //}
    }
}
