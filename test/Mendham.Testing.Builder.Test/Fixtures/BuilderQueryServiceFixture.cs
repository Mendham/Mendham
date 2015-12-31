using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.Fixtures
{
    public class BuilderQueryServiceFixture : FixtureFixture<BuilderQueryService>
    {
        public IBuilderAssemblyQueryService BuilderAssemblyQueryService { get; set; }

        public override BuilderQueryService CreateSut()
        {
            return new BuilderQueryService(BuilderAssemblyQueryService);
        }

        public override void ResetFixture()
        {
            base.ResetFixture();

            this.BuilderAssemblyQueryService = Mock.Of<IBuilderAssemblyQueryService>();
        }
    }
}
