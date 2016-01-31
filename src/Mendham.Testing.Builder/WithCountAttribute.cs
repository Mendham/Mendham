using Mendham.Testing.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class WithCountAttribute : Attribute
    {
        private readonly int _count;

        public WithCountAttribute(int count)
        {
            _count = count
                .VerifyArgumentRange(0, null, nameof(count), "WithCountAttribute count cannot be negative");
        }

        internal object CreateObject(ParameterInfo parameterInfo, IParameterInfoCreation parameterInfoCreation)
        {
            return parameterInfoCreation.Create(parameterInfo, _count);
        }
    }
}
