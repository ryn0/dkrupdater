using DKRUpdater.Core.Logging;
using System;
using System.IO;

namespace DKRUpdater.Core.FileSystem
{
    public class Directories
    {
        public static void CreateDirectoryIfNotExists(string path)
        {
            try
            {
                Log.Debug("Checking if directory: '{0}' exists...", path);

                if (Directory.Exists(path))
                {
                    Log.Debug("Directory: '{0}' exists.", path);
                    return;
                }

                Directory.CreateDirectory(path);

                Log.Debug("Created directory: '{0}'.", path);
            }
            catch (Exception ex)
            {
                var message = string.Format("Could not create directory at: '{0}'", path);

                Log.Error(message, ex.InnerException);
            }
        }
    }
}
