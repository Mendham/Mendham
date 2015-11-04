using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public static class MockableObjectBuilderExtensions
    {
        public static IBuilderForMocking<T> SetupMock<T>(this IMockableObjectBuilder<T> builder)
            where T : class
        {
            return new BuilderForMocking<T>(builder);
        }

        private class BuilderForMocking<T> : IBuilderForMocking<T>
            where T : class
        {
            private readonly IMockableObjectBuilder<T> builder;
            private readonly List<Action<Mock<T>>> mockSetupSteps;

            public BuilderForMocking(IMockableObjectBuilder<T> builder)
            {
                this.builder = builder;
                this.mockSetupSteps = new List<Action<Mock<T>>>();
            }

            public IBuilderForMocking<T> ApplyMockRule(Action<Mock<T>> mockAction)
            {
                mockSetupSteps.Add(mockAction);
                return this;
            }

            public T Build()
            {
                var mock = builder.BuildAsMock().AsMock();

                mockSetupSteps.ForEach(setupStepAction => setupStepAction(mock));

                return mock.Object;
            }
        }
    }
}