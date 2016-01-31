using System.IO;

namespace DKRUpdater.Core.FileConversion
{
    public class FileHelper
    {
        public static bool IsFileM4a(string downloadedFilePath)
        {
            var extension = Path.GetExtension(downloadedFilePath);

            return extension.ToLower() == ".m4a";
        }
    }
}
