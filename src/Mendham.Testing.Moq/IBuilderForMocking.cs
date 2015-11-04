using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public interface IBuilderForMocking<T> : IBuilder<T>
        where T : class
    {
        IBuilderForMocking<T> ApplyMockRule(Action<Mock<T>> mockAction);
    }
}
