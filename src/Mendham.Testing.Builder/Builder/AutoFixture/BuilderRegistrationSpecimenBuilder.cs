using Ploeh.AutoFixture.Kernel;
using System;

namespace Mendham.Testing.Builder.AutoFixture
{
    internal class BuilderRegistrationSpecimenBuilder : ISpecimenBuilder
    {
        private readonly static BuilderRegistrationManager builderRegistrationManager =
            new BuilderRegistrationManager();

        public object Create(object request, ISpecimenContext context)
        {
            return Create(request, context as IMendhamSpecimenContext);
        }

        private object Create(object request, IMendhamSpecimenContext context)
        {
            if (context == default(IMendhamSpecimenContext))
            {
                var errorMsg = $"{nameof(ISpecimenContext)} not of type {nameof(IMendhamSpecimenContext)} passed to {nameof(BuilderRegistrationSpecimenBuilder)}";
                throw new InvalidOperationException(errorMsg);
            }

            // If the type belongs to a known builder, then build it
            var type = request as Type;

            if (type == default(Type))
            {
                return new NoSpecimen();
            }

            var builderRegistration = builderRegistrationManager
                .GetBuilderRegistration(context.CallingAssembly);

            if (builderRegistration.IsTypeRegistered(type))
            {
                return builderRegistration.Build(type);
            }

            return new NoSpecimen();
        }
    }
}
