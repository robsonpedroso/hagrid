using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using System;
using System.Configuration;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.DTO
{
    public class FileRequisition : Requisition
    {
        [JsonProperty("file")]
        public byte[] File { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("file_extension")]
        public string FileExtension { get; set; }

        [JsonProperty("file_size")]
        public decimal FileSize { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        public FileRequisition()
        {
            Status = RequisitionStatus.Pending.AsInt();
            SaveDate = DateTime.Now;
        }

        public FileRequisition(Entities.Requisition requisition)
            : base(requisition)
        {
            var fileImport = requisition as Entities.FileRequisition;

            this.File = fileImport.File;
            this.FileExtension = fileImport.FileExtension;
            this.Filename = fileImport.Filename;
            this.FileSize = fileImport.FileSize;
            this.Url = fileImport.Url;

        }

        public FileRequisition(string filename, int bytesLength, byte[] file, Guid storeCode, string fileType)
        {
            Filename = filename.Substring(0, filename.IndexOf("."));
            FileExtension = filename.Substring(filename.IndexOf(".") + 1);
            FileSize = Convert.ToDecimal(bytesLength) / Convert.ToDecimal(1024);
            File = file;
            StoreCode = storeCode;
            RequisitionType = (RequisitionType)Enum.Parse(RequisitionType.GetType(), fileType);
            Status = RequisitionStatus.Pending.AsInt();
            SaveDate = DateTime.Now;
        }
    }
}
