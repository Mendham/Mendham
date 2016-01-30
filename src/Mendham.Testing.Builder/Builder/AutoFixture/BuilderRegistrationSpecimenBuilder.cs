using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.AutoFixture
{
    public class BuilderRegistrationSpecimenBuilder : ISpecimenBuilder
    {
        private readonly IBuilderRegistration builderRegistration;

        public BuilderRegistrationSpecimenBuilder(IBuilderRegistration builderRegistration)
        {
            builderRegistration.VerifyArgumentNotDefaultValue("Builder Registration is required");

            this.builderRegistration = builderRegistration;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;
            if (type != default(Type) && builderRegistration.IsTypeRegistered(type))
            {
                return builderRegistration.Build(type);
            }

            var pi = request as ParameterInfo;
            if (pi == default(ParameterInfo))
            {
                return new NoSpecimen(request);
            }

            if (!builderRegistration.IsTypeRegistered(pi.ParameterType))
            {
                return new NoSpecimen(request);
            }

            return builderRegistration.Build(pi.ParameterType);
        }
    }
}
