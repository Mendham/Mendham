using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.AutoFixture
{
    public class CreateWithCountBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var countWithCreateCtx = context as CreateWithCountSpecimenContext;

            if (countWithCreateCtx == default(CreateWithCountSpecimenContext))
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
