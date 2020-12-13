using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Utils;
using System;

namespace Hagrid.Core.Application
{
    public class TransferTokenApplication : AccountBaseApplication, ITransferTokenApplication
    {
        private ITransferTokenRepository transferTokenRepository;

        public TransferTokenApplication(IComponentContext context, ITransferTokenRepository transferTokenRepository)
            : base(context)
        {
            this.transferTokenRepository = transferTokenRepository;
        }

        public void Save(TransferToken token)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    if (token.Validate())
                    {
                        var _transferToken = transferTokenRepository.GetByOwner(token.OwnerCode.Value);

                        if (!_transferToken.IsNull())
                        {
                            if (!_transferToken.Validate())
                                transferTokenRepository.Delete(_transferToken);
                            else
                                throw new ArgumentException("Tranfer token válido existente.");
                        }

                        transferTokenRepository.Add(token);
                    }
                    else
                    {
                        throw new ArgumentException("Token inválido.");
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public TransferToken Get(string code)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var token = transferTokenRepository.Get(code);

                    if (!token.IsNull())
                    {
                        transferTokenRepository.Delete(token);

                        if (!token.Validate())
                            return null;
                    }

                    transaction.Commit();

                    return token;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
