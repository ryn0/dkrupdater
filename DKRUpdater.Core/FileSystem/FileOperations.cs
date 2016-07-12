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

            Log.Debug("Deleting all files in: '{0}'", directory);

            try
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    Log.Debug("Deleting file: '{0}'", file.Name);

                    file.Delete();
                }

                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Failed to delete all content in: '{0}'", directory), ex);
            }

            Log.Debug("Completed deleting all files in: '{0}'", directory);
        }

        public static string GetJsonFromPath(string path)
        {
            var json = string.Empty;

            try
            {
                Log.Debug("Reading file: '{0}'", path);

                json = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Failed to read file at: '{0}'", path), ex);
            }

            return json;
        }


        public static void DeleteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Log.Debug("File: '{0}' does not exist.", filePath);
                    return;
                }

                Log.Debug("Deleting file: '{0}'...", filePath);
                File.Delete(filePath);
                Log.Debug("Deleted file: '{0}'.", filePath);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("File '{0}' couldn't be deleted", filePath), ex);
            }
        }

        public static void RenameFile(string fromPath, string toPath)
        {
            Log.Debug("Renaming file: '{0}' to: '{1}'...", fromPath, toPath);

            DeleteFile(toPath);

            MoveFile(fromPath, toPath);

            Log.Debug("done.");
        }

        public static void MoveFile(string fromPath, string toPath)
        {
            try
            {
                Log.Debug("Moving file: '{0}' to: '{1}'", fromPath, toPath);

                new FileInfo(toPath).Directory.Create();

                File.Move(fromPath, toPath);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Error moving file: '{0}' to: '{1}'", fromPath, toPath), ex);
            }
        }
    }
}
