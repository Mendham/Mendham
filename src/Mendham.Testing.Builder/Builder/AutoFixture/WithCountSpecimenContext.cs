using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.AutoFixture
{
    public class WithCountSpecimenContext : MendhamSpecimenContext
    {
        private readonly int repeatCount;
        private bool isApplied;

        public WithCountSpecimenContext(ISpecimenBuilder builder, Assembly callingAssembly, int repeatCount)
            :base(builder, callingAssembly)
        {
            this.repeatCount = repeatCount
                .VerifyArgumentRange(nameof(repeatCount), 0, null, "Repeat count cannot be negative");

            isApplied = false;
        }

        public ISpecimenBuilder Builder
        {
            get { return builder; }
        }

        public int RepeatCount
        {
            get { return repeatCount; }
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
