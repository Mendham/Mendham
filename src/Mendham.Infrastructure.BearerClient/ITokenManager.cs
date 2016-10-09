using System.Threading;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.BearerClient
{
    public interface ITokenManager
    {
        Task<string> GetTokenAsync(CancellationToken cancellationToken);
    }
}
