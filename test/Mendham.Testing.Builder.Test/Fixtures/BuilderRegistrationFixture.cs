using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.Fixtures
{
    public class BuilderRegistrationFixture : FixtureFixture<BuilderRegistration>
    {
        public IBuilderQueryService BuilderQueryService { get; set; }
        public IBuilderAttributeResolver BuilderAttributeResolver { get; set; }

        public override BuilderRegistration CreateSut()
        {
            return new BuilderRegistration(BuilderQueryService, BuilderAttributeResolver);
        }

        public Assembly TestAssembly
        {
            get
            {
                return this.GetType().Assembly;
            }
        }

        public override void ResetFixture()
        {
            base.ResetFixture();

            BuilderQueryService = Mock.Of<IBuilderQueryService>();
            BuilderAttributeResolver = Mock.Of<IBuilderAttributeResolver>();
        }
    }
}
