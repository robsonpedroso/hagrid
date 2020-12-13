using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Infra.Providers.EntityFramework
{
    public class EFTransaction : Infra.Contracts.Repository.ITransaction
    {
        public DbContext Context { get; set; }
        public DbContextTransaction Transaction { get; set; }

        public EFTransaction(EFContext context, System.Data.IsolationLevel? isolation = null)
        {
            this.Context = context;

            this.Transaction = isolation.HasValue
                ? this.Context.Database.BeginTransaction(isolation.Value)
                : this.Context.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (this.Transaction != null)
            {
                try
                {
                    if (this.Transaction.UnderlyingTransaction.Connection != null && this.Context.ChangeTracker.HasChanges())
                    {
                        this.SaveChanges();
                        this.Transaction.Commit();
                    }
                }
                catch
                {
                    if (this.Transaction.UnderlyingTransaction.Connection != null)
                        this.Transaction.Rollback();

                    throw;
                }
                finally
                {
                    this.Transaction.Dispose();
                    this.Transaction = null;
                }
            }
        }

        public void SaveChanges()
        {
            if (this.Context.ChangeTracker.HasChanges())
                this.Context.SaveChanges();
        }

        public void Rollback()
        {
            if (this.Transaction != null)
            {
                try
                {
                    this.Transaction.Rollback();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    this.Transaction.Dispose();
                    this.Transaction = null;
                }
            }
        }

        public void Dispose()
        {
            this.Commit();
        }

        public void SetBatchSize(int size)
        {
            throw new NotImplementedException();
        }

    }
}
