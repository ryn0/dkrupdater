using System;
using System.IO;
using System.Linq;

namespace DKRUpdater.Core.StringParsing
{
    public class FileNameParsing
    {
        public static DateTime GetDateFromFilePath(string path)
        {
            var file = Path.GetFileName(path);

            var deliminter = '_';

            if (!file.Contains(deliminter))
            {
                return DateTime.MaxValue;
            }

            var datePart = file.Split(deliminter)[0];

            return Convert.ToDateTime(datePart);
        }

        public static string CleanFileName(string fileName)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }

            return fileName;
        }

    }
}
