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

            // TODO Check depth

            return context.Resolve(new FiniteSequenceRequest(manyRequest.Request,
                countWithCreateCtx.RepeatCount));
        }
    }
}
