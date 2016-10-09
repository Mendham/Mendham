using System.Threading;

namespace Mendham.Infrastructure.BearerClient
{
    public interface IMessageCancellation
    {
        CancellationToken GetCancellationToken();
    }

    public interface IWrappedMessageCancellation
    {

    }
}
