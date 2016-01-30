using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.AutoFixture
{
    public class WithCountSpecimenContext : ISpecimenContext
    {
        private readonly ISpecimenBuilder builder;
        private readonly int repeatCount;
        private bool isApplied;

        public WithCountSpecimenContext(ISpecimenBuilder builder, int repeatCount)
        {
            this.builder = builder
                .VerifyArgumentNotDefaultValue(nameof(builder));
            this.repeatCount = repeatCount
                .VerifyArgumentRange(nameof(repeatCount), 0, null, "Repeat count cannot be negative");

            isApplied = false;
        }

        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        public int RepeatCount
        {
            get { return this.repeatCount; }
        }

        public object Resolve(object request)
        {
            return this.builder.Create(request, this);
        }

        internal bool TryApply()
        {
            if (isApplied)
            {
                return false;
            }
            else
            {
                isApplied = true;
                return true;
            }
        }
    }
}
