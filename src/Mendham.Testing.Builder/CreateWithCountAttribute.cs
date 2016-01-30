using Mendham.Testing.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class CreateWithCountAttribute : Attribute
    {
        private readonly int _count;

        public CreateWithCountAttribute(int count)
        {
            _count = count
                .VerifyArgumentRange(nameof(count), 0, null, "CreateWithCountAttribute count cannot be negative");
        }

        internal object CreateObject(ParameterInfo parameterInfo, IFullObjectCreationContext objCreationCtx)
        {
            return objCreationCtx.Create(parameterInfo, _count);
        }
    }
}
