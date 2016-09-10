using System.Threading.Tasks;

namespace Mendham.Testing.AspNetCore.Test.SampleApp
{
    public interface ITestService
    {
        Task<int> GetValue();
        Task TakeAction(string value);
    }
}