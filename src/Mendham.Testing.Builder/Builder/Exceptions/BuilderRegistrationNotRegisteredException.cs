using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Exceptions
{
    public class BuilderRegistrationNotRegisteredException : Exception
    {
        public override string Message
        {
            get
            {
                return "Attempted to perform an action on BuilderRegistration prior to calling register";
            }
        }
    }
}
