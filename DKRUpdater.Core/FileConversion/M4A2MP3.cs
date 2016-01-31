using System;
using System.Diagnostics;
using DKRUpdater.Core.Logging;
using System.IO;

namespace DKRUpdater.Core
{
    public static class M4A2MP3
    {
        const string ThirdPartyExe = @"C:\thirdparty\";

        public static string ConvertMp4ToMp3(string pathOfMp4)
        {
            var copyOfpathOfMp4 = pathOfMp4;
            var pathToWav = ConvertToWav(pathOfMp4);
            var pathToMp3 = pathToWav.Replace(".wav", ".mp3");

            ConvertWavToMp3(pathToWav, pathToMp3);

            DeleteFile(copyOfpathOfMp4);
            DeleteFile(pathToWav);

            return pathToMp3;
        }

        private static void DeleteFile(string filePath)
        {
            try
            {
                Logger.Log(string.Format("Deleting file: '{0}'", filePath));
                File.Delete(filePath);
                Logger.Log(string.Format("Deleted file: '{0}'", filePath));
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("File '{0}' couldn't be deleted", filePath), ex);
            }
        }

        public static string ConvertToWav(string fromPath)
        {
            if (!File.Exists(fromPath))
            {
                Logger.LogError(string.Format("File '{0}' doesn't exist", fromPath), new Exception());
            }

            Logger.Log(string.Format("Starting M4A to WAV conversion from: '{0}'", fromPath));

            var command = ThirdPartyExe + "faad.exe";
            var args = InQuotes(fromPath);

            var process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = args;

            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("Failed M4A to WAV conversion from: '{0}'", fromPath), ex);
            }
            finally
            {
                process.Dispose();
            }

            Logger.Log(string.Format("Completed M4A to WAV conversion from: '{0}'", fromPath));

            return fromPath.Replace(".m4a", ".wav");
        }

        public static void ConvertWavToMp3(string fromPathWav, string toPathMp3)
        {
            if (!File.Exists(fromPathWav))
            {
                Logger.LogError(string.Format("File '{0}' doesn't exist", fromPathWav), new Exception());
            }

            Logger.Log(string.Format("Starting conversion to MP3 from: '{0}' to: '{1}'", fromPathWav, toPathMp3));

            string command = ThirdPartyExe + "lame.exe ";
            string args = string.Format(" -V2 --cbr -b 192 {0} {1}", InQuotes(fromPathWav), InQuotes(toPathMp3));

            var process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = args;

            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("Failed conversion to MP3 from: '{0}' to: '{1}'", fromPathWav, toPathMp3), ex);
            }
            finally
            {
                process.Dispose();
            }

            Logger.Log(string.Format("Completed conversion to MP3 from: '{0}' to: '{1}'", fromPathWav, toPathMp3));
        }

        private static string InQuotes(string withoutQuotes)
        {
            return "\"" + withoutQuotes + "\"";
        }
    }
}
