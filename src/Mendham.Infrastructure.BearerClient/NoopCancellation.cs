using System.Threading;

namespace Mendham.Infrastructure.BearerClient
{
    public class NoopCancellation : IMessageCancellation
    {
        public CancellationToken GetCancellationToken()
        {
            return CancellationToken.None;
        }
    }
}
