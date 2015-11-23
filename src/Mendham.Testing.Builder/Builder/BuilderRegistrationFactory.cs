using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public class BuilderRegistrationFactory : IBuilderRegistrationFactory
    {
        public IBuilderRegistration Create()
        {
            return new BuilderRegistration(new BuilderQueryService(), new BuilderAttributeResolver());
        }
    }
}
