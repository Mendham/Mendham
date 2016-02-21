using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.ComplexDomainGraph
{
    public interface ICountService
    {
        bool ShouldContinue();
        int CurrentCount { get; }
        bool WasMaxCountReached();
    }

    public class CountService : ICountService
    {
        private readonly IDomainEventPublisher domainEventPublisher;
        private const int MAX_COUNT = 50;

        private ReaderWriterLockSlim countLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        int _currentCount;

        public CountService(IDomainEventPublisher domainEventPublisher)
        {
            this.domainEventPublisher = domainEventPublisher;
            _currentCount = 0;
        }

        public bool ShouldContinue()
        {
            IncrementCount();
            return !WasMaxCountReached();
        }

        public int CurrentCount { get { return GetCount(); } }

        public bool WasMaxCountReached()
        {
            return GetCount() >= MAX_COUNT;
        }

        private void IncrementCount()
        {
            countLock.EnterWriteLock();
            try
            {
                _currentCount++;
            }
            finally
            {
                countLock.EnterWriteLock();
            }
        }

        private int GetCount()
        {
            countLock.EnterReadLock();
            try
            {
                return _currentCount;
            }
            finally
            {
                countLock.ExitReadLock();
            }
        }
    }
}
