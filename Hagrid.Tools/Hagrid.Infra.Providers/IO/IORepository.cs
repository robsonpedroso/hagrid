using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;

namespace Hagrid.Infra.Providers.IO
{
    public class IORepository : IIORepository
    {
        public IORepository() { }

        #region "  txt file  "

        public virtual void SaveTxtFile(string text, string finalFileName, string fullDirectory)
        {
            SaveFile(text, finalFileName, fullDirectory, "txt");
        }

        public virtual void DeleteTxtFile(string finalFileName, string fullDirectory)
        {
            DeleteFile(finalFileName, fullDirectory, "txt");
        }

        #endregion

        #region "  csv file  "

        public virtual void SaveCsvFile(string text, string finalFileName, string fullDirectory)
        {
            SaveFile(text, finalFileName, fullDirectory, "csv");
        }

        public virtual void DeleteCsvFile(string finalFileName, string fullDirectory)
        {
            DeleteFile(finalFileName, fullDirectory, "csv");
        }

        public virtual void SaveXmlFile(string text, string finalFileName, string fullDirectory)
        {
            SaveFile(text, finalFileName, fullDirectory, "xml");
        }

        public virtual void DeleteXmlFile(string finalFileName, string fullDirectory)
        {
            DeleteFile(finalFileName, fullDirectory, "xml");
        }

        #endregion

        #region "  byte[]  "

        public virtual void SaveFile(byte[] file, string finalFileName, string fullDirectory, string extension)
        {
            ValidateData(finalFileName, fullDirectory);

            if (!Directory.Exists(fullDirectory))
                Directory.CreateDirectory(fullDirectory);

            string filefullName = string.Format(@"{0}\{1}.{2}", fullDirectory.ToLower(), finalFileName.ToLower(), extension);

            if (File.Exists(filefullName))
                File.Delete(filefullName);

            FileStream fileStream = File.Create(filefullName);
            fileStream.Write(file, 0, file.Length);

            fileStream.Close();
            fileStream.Dispose();
        }

        public void DeleteFile(string finalFileName, string fullDirectory, string extension)
        {
            ValidateData(finalFileName, fullDirectory);

            string filefullName = string.Format(@"{0}\{1}.{2}", fullDirectory.ToLower(), finalFileName.ToLower(), extension);

            if (File.Exists(filefullName))
                File.Delete(filefullName);
        }

        #endregion

        private void SaveFile(string text, string finalFileName, string fullDirectory, string extension)
        {
            ValidateData(finalFileName, fullDirectory);

            if (!Directory.Exists(fullDirectory))
                Directory.CreateDirectory(fullDirectory);

            string filefullName = string.Format(@"{0}\{1}.{2}", fullDirectory.ToLower(), finalFileName.ToLower(), extension);

            if (File.Exists(filefullName))
                File.Delete(filefullName);

            StreamWriter file = new StreamWriter(filefullName);
            file.WriteLine(text);

            file.Close();
        }

        private void ValidateData(string finalFileName, string fullDirectory)
        {
            if (string.IsNullOrWhiteSpace(fullDirectory))
                throw new ArgumentException("Diretório inválido");

            if (string.IsNullOrWhiteSpace(finalFileName))
                throw new ArgumentException("Nome do arquivo inválido");
        }

        public virtual FileInfo[] GetFiles(string fullDirectory, string fileName)
        {
            DirectoryInfo di = new DirectoryInfo(fullDirectory);

            if (!fileName.IsNullOrWhiteSpace())
                return di.GetFiles(fileName);
            else
                return di.GetFiles();
        }

        public virtual byte[] ReadAllBytes(string fullDirectory, string fileName, string extension)
        {
            return File.ReadAllBytes(@"{0}\{1}.{2}".ToFormat(fullDirectory, fileName, extension));
        }

        public virtual void AppendText(string text, string finalFileName, string fullDirectory, string extension)
        {
            using (StreamWriter sw = File.AppendText(string.Format(@"{0}\{1}.{2}", fullDirectory.ToLower(), finalFileName.ToLower(), extension)))
            {
                sw.WriteLine(text);
            }
        }

        public virtual string[] ReadAllLines(string fileName, string directory, string extension, Encoding encoding)
        {
            ValidateData(fileName, directory);

            if(extension.IsNullOrWhiteSpace())
                throw new ArgumentException("Extensão do arquivo inválida");

            string filefullName = string.Format(@"{0}\{1}.{2}", directory.ToLower(), fileName.ToLower(), extension);

            if (!File.Exists(filefullName))
                throw new ArgumentException("Arquivo não encontrado");

            return File.ReadAllLines(filefullName, encoding);
        }
    }
}
