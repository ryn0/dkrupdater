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
            var maxLengthOfFileName = 75;
            var extension = Path.GetExtension(fileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var shortFileNameWithoutExtension = Truncate(fileNameWithoutExtension, maxLengthOfFileName);

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                shortFileNameWithoutExtension = shortFileNameWithoutExtension.Replace(c, '_');
            }

            return shortFileNameWithoutExtension + extension;
        }

        private static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }

            return source;
        }
    }
}
