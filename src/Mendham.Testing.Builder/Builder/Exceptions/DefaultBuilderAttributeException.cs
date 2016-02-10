using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Exceptions
{
    public abstract class DefaultBuilderAttributeException : Exception
    {
        public Type BuilderType { get; private set; }

        public DefaultBuilderAttributeException(Type builderType)
        {
            this.BuilderType = builderType;
        }
    }
}
