using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application.Contracts
{
    public interface IRequisitionApplication
    {
        DTO.DBRequisition SaveDB(DTO.DBRequisition requisition);

        void SaveFile(IEnumerable<DTO.FileRequisition> fileImport);

        string DownloadCsv(Guid requisitionCode);

        IEnumerable<DTO.Requisition> GetByStore(Guid storeCode, bool withErrors = false);

        void Delete(Guid code);

        void Process();
    }
}
