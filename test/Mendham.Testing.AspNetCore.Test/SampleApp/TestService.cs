using System.Threading.Tasks;

namespace Mendham.Testing.AspNetCore.Test.SampleApp
{
    public class TestService : ITestService
    {
        private readonly IDependency1 _dependency1;

        public const int DefaultGetValue = 100;
        public const string StringForTrueAction = "abc";

        public TestService(IDependency1 dependency1)
        {
            _dependency1 = dependency1;
        }

        public Task<int> GetValue()
        {
            return Task.FromResult(100);
        }

        public Task<bool> TakeAction(string value)
        {
            return Task.FromResult(value == StringForTrueAction);
        }

        public Task<int> GetDependentValueAsync()
        {
            int value = _dependency1.GetValue();
            return Task.FromResult(value);
        }
    }
}
