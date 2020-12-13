using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using Hagrid.Infra.Utils;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Services
{
    public class BlacklistService : IBlacklistService
    {
        private IBlacklistRepository blacklistRep;

        public BlacklistService(IBlacklistRepository blacklistRep)
        {
            this.blacklistRep = blacklistRep;
        }

        public Blacklist Get(Guid code)
        {
            return blacklistRep.Get(code);
        }

        public Blacklist Block(Blacklist blacklist)
        {
            blacklist.Events.ForEach(x => x.Blocked = true);
            blacklist.Blocked = true;

            var result = blacklistRep.GetUser(blacklist.AccountCode, blacklist.StoreCode);
            if (!result.IsNull())
            {
                result.Blocked = blacklist.Blocked;
                result.UpdateDate = DateTime.Now;
                result.Events.AddRange(blacklist.Events);
                return blacklistRep.Save(result);
            }
            else
                return blacklistRep.Save(blacklist);
        }

        public Blacklist Unlock(Blacklist blacklist)
        {
            blacklist.Events.ForEach(x => x.Blocked = false);
            blacklist.Blocked = false;

            var result = blacklistRep.GetUser(blacklist.AccountCode, blacklist.StoreCode);
            if (!result.IsNull())
            {
                result.Blocked = blacklist.Blocked;
                result.UpdateDate = DateTime.Now;
                result.Events.AddRange(blacklist.Events);
                return blacklistRep.Save(result);
            }
            else
                return blacklistRep.Save(blacklist);
        }

        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() {
                blacklistRep
            };
        }

        #endregion
    }
}
