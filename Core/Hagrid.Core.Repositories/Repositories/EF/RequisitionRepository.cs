using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Providers.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Hagrid.Infra.Contracts.Repository;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class RequisitionRepository : BaseRepository<Requisition, CustomerContext, Guid>, IRequisitionRepository
    {
        public RequisitionRepository(IConnection connection) : base(connection) { }

        public override Requisition Save(Requisition obj)
        {
#if DEBUG

            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif
            return base.Save(obj);
        }

        public RequisitionError SaveError(RequisitionError error)
        {
            return Context.Set<RequisitionError>().Add(error);
        }

        public virtual IEnumerable<Requisition> GetByStore(Guid storeCode, bool withErrors = false)
        {
            var result = Collection.Where(c => c.Store.Code == storeCode && c.Removed == false)
                .Include(c => c.Store);

            if (withErrors)
                result = result.Include(c => c.RequisitionErrors);

            return result.ToList();
        }

        public override Requisition Get(Guid code)
        {
            return Collection.Where(c => c.Code == code && c.Removed == false)
                .Include(c => c.Store)
                .FirstOrDefault();
        }
        public Requisition Get(Guid code, bool withErrors = false)
        {
            var result = Collection.Where(c => c.Code == code && c.Removed == false);

            if (withErrors)
                result = result.Include(c => c.RequisitionErrors);

            return result.FirstOrDefault();
        }

        public IEnumerable<Requisition> GetByStatus(RequisitionStatus status)
        {
            return Collection.Where(c => c.Status == status && c.Removed == false)
                .Include(c => c.Store)
                .ToList();
        }

        public IEnumerable<Requisition> GetStatusAndSetProcessing(RequisitionStatus status)
        {
            var result = Collection.Where(c => c.Status == status && c.Removed == false)
                .Include(c => c.Store)
                .ToList();

            result.ForEach(item =>
            {
                item.Status = RequisitionStatus.Processing;
                item.UpdateDate = DateTime.Now;

                Collection.Attach(item);
                Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            });

            Context.SaveChanges();

            return result;
        }
    }
}
