using System.IO;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class IOExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirImage"></param>
        /// <returns></returns>
        public static string GetImageBase64(this string dirImage)
        {
            if (File.Exists(dirImage))
                return File.ReadAllBytes(dirImage).AsBase64String();
            else
                return string.Empty;
        }
    }
}
