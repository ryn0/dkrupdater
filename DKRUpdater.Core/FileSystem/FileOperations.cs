using DKRUpdater.Core.Logging;
using System;
using System.IO;

namespace DKRUpdater.Core.FileSystem
{
    public class FileOperations
    {
        public static void DeleteAllFilesInDirectory(string directory)
        {
            var di = new DirectoryInfo(directory);

            Logger.Log(string.Format("Deleting all files in: '{0}'", directory));

            try
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    Logger.Log(string.Format("Deleting file: '{0}'", file.Name));

                    file.Delete();
                }

                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("Failed to delete all content in: '{0}'", directory), ex);
            }

            Logger.Log(string.Format("Completed deleting all files in: '{0}'", directory));
        }


        public static void MoveFile(string fromPath, string toPath)
        {
            try
            {
                Logger.Log(string.Format("Moving file: '{0}' to: '{1}'", fromPath, toPath));

                new FileInfo(toPath).Directory.Create();

                File.Move(fromPath, toPath);
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("Error moving file: '{0}' to: '{1}'", fromPath, toPath), ex);
            }
        }
    }
}
