using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Exceptions
{
    public class InvalidDefaultBuilderOverrideException : DefaultBuilderAttributeException
    {
        public Type TypeOverride { get; private set; }
        public Type TypeToBuild { get; private set; }

        public InvalidDefaultBuilderOverrideException(Type builderType, Type typeOverride, Type typeToBuilder)
            : base(builderType)
        {
            this.TypeOverride = typeOverride;
            this.TypeToBuild = typeToBuilder;
        }

        public override string Message
        {
            get
            {
                return string.Format(
                    CultureInfo.CurrentCulture,
                    "{0} attempted to apply an override that is not valid for the builder applied. {1} is not assignable from {2}. See attribute applied at {3}.",
                    typeof(DefaultBuilderAttribute).Name,
                    TypeOverride.FullName,
                    TypeToBuild.FullName,
                    BuilderType.FullName);
            }
        }
    }
}
