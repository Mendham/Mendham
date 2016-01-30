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
                .VerifyArgumentRange(nameof(count), 0, null, "WithCountAttribute count cannot be negative");
        }

        internal object CreateObject(ParameterInfo parameterInfo, IObjectCreationContext objCreationCtx)
        {
            return objCreationCtx.Create(parameterInfo, _count);
        }
    }
}
