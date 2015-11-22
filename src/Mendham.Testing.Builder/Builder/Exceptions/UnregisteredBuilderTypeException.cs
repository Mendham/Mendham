using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Exceptions
{
    public class UnregisteredBuilderTypeException : Exception
    {
        public Type TypeAttemptedToBuild { get; private set; }

        public UnregisteredBuilderTypeException(Type typeAttemptedToBuild)
        {
            this.TypeAttemptedToBuild = typeAttemptedToBuild;
        }

        public override string Message
        {
            get
            {
                return string.Format(
                    CultureInfo.CurrentCulture,
                    "Attempted to build to build type {0} which is not defined in {1}",
                    TypeAttemptedToBuild.FullName,
                    typeof(BuilderRegistration).Name);
            }
        }
    }
}
