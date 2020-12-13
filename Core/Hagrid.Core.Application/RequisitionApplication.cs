using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO = Hagrid.Core.Domain.DTO;
using Autofac;

namespace Hagrid.Core.Application
{
    public class RequisitionApplication : AccountBaseApplication, IRequisitionApplication
    {
        private IRequisitionRepository requisitionRepository;
        private IRequisitionService requisitionService;
        private ICustomerImportService customerImportService;
        private IStoreRepository storeRepository;

        public RequisitionApplication(
            ILifetimeScope context, 
            IRequisitionService requisitionService, 
            IRequisitionRepository requisitionRepository,
            ICustomerImportService customerImportService,
            IStoreRepository storeRepository)
            : base(context)
        {
            this.requisitionService = requisitionService;
            this.requisitionRepository = requisitionRepository;
            this.customerImportService = customerImportService;
            this.storeRepository = storeRepository;
        }

        public DTO.DBRequisition SaveDB(DTO.DBRequisition requisition)
        {
            Requisition result;
            requisition.isValidate();

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    result = requisitionService.Save(new DBRequisition(requisition));
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return new DTO.DBRequisition(result);
        }

        public void SaveFile(IEnumerable<DTO.FileRequisition> filesRequisition)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    filesRequisition.ForEach(fileImport => requisitionService.SaveFile(new FileRequisition(fileImport)));
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IEnumerable<DTO.Requisition> GetByStore(Guid storeCode, bool withErrors = false)
        {
            var result = requisitionRepository.GetByStore(storeCode, withErrors);

            return result.Select(p => p.GetResult()).ToList();            
        }

        public string DownloadCsv(Guid requisitionCode)
        {
            var result = requisitionService.SaveCSVFile(requisitionCode);
            return result;
        }

        public void Delete(Guid code)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                requisitionService.Delete(code);
                transaction.Commit();
            }
        }

        public void Process()
        {
            var requisitions = requisitionRepository.GetStatusAndSetProcessing(RequisitionStatus.Pending);

            requisitions.ForEach(requisition =>
            {
                RequisitionStatus status;

                try
                {
                    var requisitionProcesssing = context.ResolveNamed<IRequisitionProcessingService>(requisition.RequisitionType.ToLower());

                    ApplicationStore appSto = null;

                    if (requisition.RequisitionType == RequisitionType.ImportInternalAccounts)
                    {
                        requisitionProcesssing.ImportCustomerToImportDb(requisition);
                        appSto = requisitionProcesssing.GetApplicationStore(requisition.Store.Code);
                    }

                    int skip = 0;
                    object[] accounts = null;

                    do
                    {
                        if (!accounts.IsNull())
                            skip += accounts.Count() == Config.ProcessImportNumberRecordsPerCommit ? Config.ProcessImportNumberRecordsPerCommit : accounts.Count();

                        accounts = requisitionProcesssing.GetAccounts(requisition, skip, Config.ProcessImportNumberRecordsPerCommit);

                        Parallel.ForEach(accounts,
                            new ParallelOptions() { MaxDegreeOfParallelism = Config.ProcessImportMaxDegreeOfParallelism },
                            account =>
                            {
                                using (var scope = ((ILifetimeScope)context).BeginLifetimeScope())
                                {
                                    var currentRequisitionProcesssing = scope.ResolveNamed<IRequisitionProcessingService>(requisition.RequisitionType.ToLower());

                                    bool isValid = false;

                                    using (var connection = scope.Resolve<IConnection>())
                                    using (var transacton = connection.BeginTransaction())
                                    {
                                        try
                                        {
                                            isValid = currentRequisitionProcesssing.SaveAccount(requisition, account, appSto, connection);

                                            if (isValid)
                                            {
                                                transacton.Commit();
                                            }
                                            else
                                            {
                                                transacton.Rollback();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            if (!ex.InnerException.IsNull() && !ex.InnerException.Message.IsNullorEmpty())
                                                requisition.RequisitionErrors.Add(new RequisitionError() { ErrorMessages = new List<string>() { ex.InnerException.ToString() } });
                                            else
                                                requisition.RequisitionErrors.Add(new RequisitionError() { ErrorMessages = new List<string>() { ex.ToString() } });

                                            transacton.Rollback();
                                        }
                                    }
                                }
                            });
                    }
                    while (requisition.RequisitionType == RequisitionType.ImportInternalAccounts && !accounts.IsNull() && accounts.Count() > 0);

                    using (var transaction = Connection.BeginTransaction())
                    {
                        requisition.Store = storeRepository.Get(requisition.Store.Code);

                        requisitionRepository.Save(requisition);

                        if (requisition.RequisitionType == RequisitionType.ImportInternalAccounts)
                        {
                            requisitionProcesssing.ClearImportDb(requisition);
                        }
                    }
                }
                catch (Exception ex)
                {
                    requisition.RequisitionErrors.Add(new RequisitionError()
                    {
                        ErrorMessages = new List<string>() {
                                    "Erro desconhecido, tente importar novamente!",
                                    ex.Message,
                                    ex.StackTrace
                            }
                    });
                }

                using (var transaction = Connection.BeginTransaction())
                {
                    status = requisition.RequisitionErrors.Count > 0 ? RequisitionStatus.Failure : RequisitionStatus.Success;

                    requisitionService.UpdateStatus(requisition, status);
                }
            });
        }
    }
}