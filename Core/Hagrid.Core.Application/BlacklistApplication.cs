using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using System;
using DTO = Hagrid.Core.Domain.DTO;
using Entities = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Application
{
    public class BlacklistApplication : AccountBaseApplication, IBlacklistApplication
    {
        private IBlacklistService service;
        public BlacklistApplication(IComponentContext context, IBlacklistService service)
            : base(context)
        {
            this.service = service;
        }

        public DTO.Blacklist Block(DTO.Blacklist blacklist)
        {
            Blacklist result;
            Blacklist save;

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    save = service.Block(new Entities.Blacklist(blacklist));
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            result = service.Get(save.Code);

            return new DTO.Blacklist(result);
        }

        public DTO.Blacklist Get(Guid code)
        {
            return new DTO.Blacklist(service.Get(code));
        }

        public DTO.Blacklist Unlock(DTO.Blacklist blacklist)
        {
            Blacklist result;
            Blacklist save;

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    save = service.Unlock(new Entities.Blacklist(blacklist));
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            result = service.Get(save.Code);

            return new DTO.Blacklist(result);
        }
    }
}
