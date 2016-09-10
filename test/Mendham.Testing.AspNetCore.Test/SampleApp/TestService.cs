using System.Threading.Tasks;

namespace Mendham.Testing.AspNetCore.Test.SampleApp
{
    public class TestService : ITestService
    {
        public Task<int> GetValue()
        {
            return Task.FromResult(100);
        }

        public Task TakeAction(string value)
        {
            return Task.FromResult(0);
        }
    }
}
