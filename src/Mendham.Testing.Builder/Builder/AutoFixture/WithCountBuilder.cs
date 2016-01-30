using Ploeh.AutoFixture.Kernel;

namespace Mendham.Testing.Builder.AutoFixture
{
    internal class WithCountBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var countWithCreateCtx = context as WithCountSpecimenContext;

            if (countWithCreateCtx == default(WithCountSpecimenContext))
                return new NoSpecimen();

            var manyRequest = request as MultipleRequest;

            if (manyRequest == default(MultipleRequest))
                return new NoSpecimen();

            // Only the first level should apply the count. The rest should use default
            if (!countWithCreateCtx.TryApply())
                return new NoSpecimen();

            return context.Resolve(new FiniteSequenceRequest(manyRequest.Request,
                countWithCreateCtx.RepeatCount));
        }
    }
}
