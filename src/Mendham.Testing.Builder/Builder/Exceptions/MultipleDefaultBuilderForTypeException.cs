using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Exceptions
{
    public class MultipleDefaultBuilderForTypeException : DefaultBuilderAttributeException
    {
        public Type AdditionalBuilderType { get; private set; }
        public Type TypeToBuild { get; private set; }

        public MultipleDefaultBuilderForTypeException(Type builderType, Type additionalBuilderType, Type typeToBuilder)
            : base(builderType)
        {
            this.AdditionalBuilderType = additionalBuilderType;
            this.TypeToBuild = typeToBuilder;
        }

        public override string Message
        {
            get
            {
                return string.Format(
                    CultureInfo.CurrentCulture,
                    "Multiple builders for type {0} have been set configured using attribute {3}. Builders {1} and {2} cannot share be set to both build the same type.",
                    TypeToBuild.FullName,
                    BuilderType.FullName,
                    AdditionalBuilderType.FullName,
                    typeof(DefaultBuilderAttribute).Name);
            }
        }
    }
}
