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
            // If the type belongs to a known builder, then build it
            var type = request as Type;

            if (type == default(Type))
            {
                return new NoSpecimen();
            }
            else if (builderRegistration.IsTypeRegistered(type))
            {
                return builderRegistration.Build(type);
            }

            return new NoSpecimen();
        }
    }
}
