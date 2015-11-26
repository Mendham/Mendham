using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public class BuilderAttributeResolver : IBuilderAttributeResolver
    {
        public IEnumerable<MendhamBuilderAttribute> GetAttributesAppliedToBuilder(Type builderType)
        {
            return GetAttributesAppliedToClass<MendhamBuilderAttribute>(builderType);
        }

        public IEnumerable<TAttribute> GetAttributesAppliedToClass<TAttribute>(Type classWithAttribute)
            where TAttribute : Attribute
        {
            return classWithAttribute
                .GetTypeInfo()
                .GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>();
        }
    }
}
