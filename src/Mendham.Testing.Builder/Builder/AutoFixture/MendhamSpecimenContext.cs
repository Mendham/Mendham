using Ploeh.AutoFixture.Kernel;
using System.Reflection;

namespace Mendham.Testing.Builder.AutoFixture
{
    public class MendhamSpecimenContext : IMendhamSpecimenContext
    {
        protected readonly ISpecimenBuilder builder;
        private readonly Assembly callingAssembly;

        public MendhamSpecimenContext(ISpecimenBuilder builder, Assembly callingAssembly)
        {
            this.builder = builder;
            this.callingAssembly = callingAssembly;
        }

        public object Resolve(object request)
        {
            return this.builder.Create(request, this);
        }

        public Assembly CallingAssembly
        {
            get { return this.callingAssembly; }
        }
    }
}
