using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Infra.Contracts.Repository
{
    /// <summary>
    /// kct de git
    /// </summary>
    public interface IIORepository
    {
        void SaveTxtFile(string text, string finalFileName, string fullDirectory);

        void DeleteTxtFile(string finalFileName, string fullDirectory);

        void SaveCsvFile(string text, string finalFileName, string fullDirectory);

        void DeleteCsvFile(string finalFileName, string fullDirectory);

        void SaveXmlFile(string text, string finalFileName, string fullDirectory);

        void DeleteXmlFile(string finalFileName, string fullDirectory);

        void SaveFile(byte[] file, string finalFileName, string fullDirectory, string extension);

        FileInfo[] GetFiles(string fullDirectory, string fileName);

        void DeleteFile(string finalFileName, string fullDirectory, string extension);

        byte[] ReadAllBytes(string fullDirectory, string fileName, string extension);

        void AppendText(string text, string finalFileName, string fullDirectory, string extension);

        string[] ReadAllLines(string fileName, string directory, string extension, Encoding encoding);
    }
}
