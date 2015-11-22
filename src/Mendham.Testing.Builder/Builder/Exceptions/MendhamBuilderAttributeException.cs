using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Exceptions
{
    public abstract class MendhamBuilderAttributeException : Exception
    {
        public Type BuilderType { get; private set; }

        public MendhamBuilderAttributeException(Type builderType)
        {
            this.BuilderType = builderType;
        }
    }
}
