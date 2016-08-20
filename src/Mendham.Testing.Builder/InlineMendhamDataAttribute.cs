using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Mendham.Testing
{
    [DataDiscoverer("Mendham.Testing.Xunit.DisableDiscoveryDataDiscoverer",
        "Mendham.Testing.Builder")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class InlineMendhamDataAttribute : DataAttribute
    {
        private readonly object[] _values;

        public InlineMendhamDataAttribute(params object[] values)
        {
            _values = values;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            testMethod.VerifyArgumentNotDefaultValue("Test method is required");

            var inlineAttributeData = new InlineDataAttribute(_values)
                .GetData(testMethod);
            var mendhamDataAttributeData = new MendhamDataAttribute()
                .GetData(testMethod);

            return inlineAttributeData
                .Zip(mendhamDataAttributeData, (inline, md) => CombineValues(inline, md));
        }

        private static object[] CombineValues(params object[][] dataSets)
        {
            return Enumerable
                .Range(0, dataSets.Select(a => a.Count()).Max())
                .Select(idx => dataSets
                    .First(dataSet => dataSet.Count() > idx)[idx])
                .ToArray();
        }

    }
}
