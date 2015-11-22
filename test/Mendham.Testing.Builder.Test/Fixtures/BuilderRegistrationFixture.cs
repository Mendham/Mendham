using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.Fixtures
{
    public class BuilderRegistrationFixture : BaseFixture<BuilderRegistration>
    {
        public IBuilderLookup BuilderLookup { get; set; }
        public IBuilderAttributeResolver BuilderAttributeResolver { get; set; }

        public override BuilderRegistration CreateSut()
        {
            return new BuilderRegistration();
        }

        public override void ResetFixture()
        {
            base.ResetFixture();

            BuilderLookup = Mock.Of<IBuilderLookup>();
            BuilderAttributeResolver = Mock.Of<IBuilderAttributeResolver>();
        }
    }
}
