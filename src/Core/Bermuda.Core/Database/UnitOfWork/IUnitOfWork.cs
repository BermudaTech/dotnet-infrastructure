using System;
using System.Data;

namespace Bermuda.Core.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        TDbContext GetCurrentDbContext<TDbContext>();

        void Begin(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        void Commit();
    }
}
