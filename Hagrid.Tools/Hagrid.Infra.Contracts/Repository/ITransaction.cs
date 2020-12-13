using System;

namespace Hagrid.Infra.Contracts.Repository
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
        void SetBatchSize(int size);

    }
}
