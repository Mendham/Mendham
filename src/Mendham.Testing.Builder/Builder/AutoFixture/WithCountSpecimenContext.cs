using Ploeh.AutoFixture.Kernel;
using System.Reflection;

namespace Mendham.Testing.Builder.AutoFixture
{
    internal class WithCountSpecimenContext : MendhamSpecimenContext
    {
        private readonly int repeatCount;
        private bool isApplied;

        public WithCountSpecimenContext(ISpecimenBuilder builder, Assembly callingAssembly, int repeatCount)
            :base(builder, callingAssembly)
        {
            this.repeatCount = repeatCount
                .VerifyArgumentRange(0, null, nameof(repeatCount), "Repeat count cannot be negative");

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
