using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;
using Hagrid.Infra.Utils;
using DTO = Hagrid.Core.Domain.DTO;
using Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.Entities
{
    public class FileRequisition : Requisition
    {
        [NotMapped]
        [JsonIgnore]
        public const string DirFile = @"assets\fileimport\import";

        [NotMapped]
        [JsonIgnore]
        public const string UrlFile = "assets/fileimport/import";

        public FileRequisition()
        {

        }

        [NotMapped]
        [JsonIgnore]
        public byte[] File { get; set; }

        [NotMapped]
        public string Filename { get; set; }

        [NotMapped]
        public string FileExtension { get; set; }

        [NotMapped]
        public decimal FileSize { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string Dir
        {
            get
            {
                return Path.Combine(Config.DirRoot, DirFile, this.Store.Code.ToString());
            }
        }

        [NotMapped]
        [JsonIgnore]
        public string Url
        {
            get
            {
                return string.Format("{0}/{1}/{2}/{3}.{4}",
                    Config.UrlRoot, 
                    UrlFile,
                    this.Store.Code,
                    this.Code,
                    this.FileExtension
                );
            }
        }

        [JsonIgnore]
        public override string ObjectSerialized
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }
            set
            {
                FileRequisition savedRequisition = string.IsNullOrEmpty(value)
                        ? new FileRequisition()
                        : JsonConvert.DeserializeObject<FileRequisition>(value);

                this.FileExtension = savedRequisition.FileExtension;
                this.Filename = savedRequisition.Filename;
                this.FileExtension = savedRequisition.FileExtension;
                this.FileSize = savedRequisition.FileSize;
            }
        }

        public FileRequisition(DTO.FileRequisition requisition)
            : base(requisition)
        {
            this.File = requisition.File;
            this.FileExtension = requisition.FileExtension;
            this.Filename = requisition.Filename;
            this.FileSize = requisition.FileSize;
        }

        public override DTO.Requisition GetResult()
        {
            return new DTO.FileRequisition(this);
        }
    }
}

