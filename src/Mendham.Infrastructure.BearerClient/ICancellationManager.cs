using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.BearerClient
{
    public interface ICancellationManager
    {
        CancellationToken GetCancellationToken();

        Exception HandleCancellation(OperationCanceledException exception);
    }
}
