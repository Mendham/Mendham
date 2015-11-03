using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain
{
    public interface IDomainFacade
    {
        Task RaiseEventAsync<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : class, IDomainEvent;
    }
}
