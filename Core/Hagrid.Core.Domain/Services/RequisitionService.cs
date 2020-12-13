using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Hagrid.Core.Domain.Services
{
    public class RequisitionService : IRequisitionService
    {
        private IRequisitionRepository requisitionRepository;
        private IIORepository ioRepository;
        private IStoreRepository storeRepository;

        public RequisitionService(IRequisitionRepository pfileImportRepository, IIORepository ioRepository, IStoreRepository pIStoreRepository)
        {
            this.requisitionRepository = pfileImportRepository;
            this.ioRepository = ioRepository;
            this.storeRepository = pIStoreRepository;
        }

        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() { requisitionRepository, storeRepository };
        }

        #endregion

        public Requisition Save(Requisition requisition)
        {
            if (requisition.Code.IsEmpty())
                requisition.Code = Guid.NewGuid();

            var store = storeRepository.Get(requisition.Store.Code);
            if (store != null)
                requisition.Store = store;
            else
                throw new ArgumentException("Loja não encontrada");

            if (requisition is DBRequisition)
            {
                var dbReq = requisition as DBRequisition;

                var exists = requisitionRepository.GetByStore(requisition.Store.Code).Any(x =>
                {
                    if (x.Status == RequisitionStatus.Pending && x.RequisitionType == RequisitionType.ImportInternalAccounts)
                    {
                        var dbrequisition = x as DBRequisition;

                        if (dbrequisition.LinkedServerName == dbReq.LinkedServerName && dbrequisition.DataBaseName == dbReq.DataBaseName)
                            return true;
                    }

                    return false;
                });

                if (exists)
                    throw new ArgumentException(string.Format("Já existe uma requisição pendente para o ambiente {0} e banco de dados {1}", dbReq.LinkedServerName, dbReq.DataBaseName));
            }

            return requisitionRepository.Save(requisition);
        }

        public void SaveFile(Requisition requisition)
        {
            FileRequisition fileRequisition = requisition as FileRequisition;
            if (fileRequisition.File.IsNull() || fileRequisition.File.Length.IsZero() || fileRequisition.FileSize.IsZero())
                throw new ArgumentException("Arquivo inválido");

            if (fileRequisition.FileExtension.IsNullOrWhiteSpace())
                throw new ArgumentException("Extensão do arquivo inválida");

            if (requisition.Code.IsEmpty())
                requisition.Code = Guid.NewGuid();

            fileRequisition.Filename = requisition.Code.ToString();

            var result = Save(requisition);

            ioRepository.SaveFile(fileRequisition.File, result.Code.ToString(), fileRequisition.Dir, fileRequisition.FileExtension);
        }

        public string SaveCSVFile(Guid requisitionCode)
        {
            if (requisitionCode.IsEmpty())
                throw new ArgumentException("Código da requisição inválida");

            var assetsCsvDir = ConfigurationManager.AppSettings["AssetsCsvDir"].AsString("");
            var assetsCsvURL = ConfigurationManager.AppSettings["AssetsCsvURL"].AsString("");

            var dir = System.IO.Path.Combine(ConfigurationManager.AppSettings["DirRoot"].AsString(""), assetsCsvDir);
            var fileName = requisitionCode.ToString();

            if (!System.IO.File.Exists(string.Format("{0}\\{1}.{2}", dir, fileName, "csv")))
            {
                var requisition = requisitionRepository.Get(requisitionCode, true).GetResult();
                if (requisition.Errors.IsNull() || requisition.Errors.Count() == 0)
                    throw new ArgumentException("Nenhum erro encontrado para a requisição");

                using (var sw = new System.IO.StreamWriter(string.Format("{0}\\{1}.{2}", dir, fileName, "csv"), false, Encoding.UTF8))
                {
                    requisition.Errors.ForEach(x =>
                    {
                        sw.WriteLine(string.Format("{0},{1},{2}", x.Name, x.Email, string.Join("|", x.ErrorMessages)));
                    });
                }
            }

            var Url = string.Format("{0}/{1}/{2}.{3}",
                    ConfigurationManager.AppSettings["UrlRoot"].AsString(""),
                    assetsCsvURL,
                    requisitionCode.ToString(),
                    "csv");

            return Url;
        }

        public void Delete(Guid pCode)
        {
            if (pCode.IsEmpty())
                throw new ArgumentException("Código do arquivo inválido");

            var requisition = requisitionRepository.Get(pCode);

            if (requisition.Status != RequisitionStatus.Pending)
                throw new ArgumentException("O arquivo não pode ser removido porque não está com o status pendente");

            requisition.Removed = true;
            requisition.UpdateDate = DateTime.Now;

            requisitionRepository.Update(requisition);

            if (requisition.RequisitionType == RequisitionType.ImportExternalAccounts)
            {
                var fileRequisition = requisition as FileRequisition;
                ioRepository.DeleteFile(requisition.Code.ToString(), fileRequisition.Dir, fileRequisition.FileExtension);
            }
        }

        public void UpdateStatus(Requisition requisition, RequisitionStatus status)
        {
            if (requisition.Code.IsEmpty())
                throw new ArgumentException("Código do arquivo inválido");

            if (requisition.Store.Code.IsEmpty())
                throw new ArgumentException("Loja inválida");

            switch (status)
            {
                case RequisitionStatus.Pending:
                    throw new ArgumentException("O arquivo não pode se atualizado para status \"Pendente\"");
                default:
                    break;
            }

            requisition.Status = status;
            requisition.UpdateDate = DateTime.Now;

            requisitionRepository.Update(requisition);
        }
    }
}
