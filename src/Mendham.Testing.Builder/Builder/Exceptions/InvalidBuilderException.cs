using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Exceptions
{
    public class InvalidBuilderException : MendhamBuilderAttributeException
    {
        public InvalidBuilderException(Type builderType)
            : base(builderType)
        { }

        public override string Message
        {
            get
            {
                return string.Format(
                    CultureInfo.CurrentCulture,
                    "Incorrect attribute assignment to class {0}. Attribute {1} can only be applied to classes that implement interface {2}",
                    BuilderType.FullName,
                    typeof(MendhamBuilderAttribute).FullName,
                    typeof(IBuilder<>).FullName);
            }
        }
    }
}
