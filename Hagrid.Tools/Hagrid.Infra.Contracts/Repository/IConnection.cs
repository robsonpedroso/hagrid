using System;

namespace Hagrid.Infra.Contracts.Repository
{
    public interface IConnection : IDisposable
    {
        bool IsOpen { get; }

        void Open(bool setCommitFlushMode = false);
        void OpenStateless(bool setCommitFlushMode = false);
        void Close();
        bool IsDispose();
        ITransaction BeginTransaction(string isolation = null, bool setCommitFlushMode = false);
    }
}
