using System;
using System.Diagnostics;
using DKRUpdater.Core.Logging;
using System.IO;

namespace DKRUpdater.Core
{
    public static class M4A2MP3
    {
        static string PathToExe = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        private const string mp3 = ".mp3";
        private const string wav = ".wav";
        private const string m4a = ".m4a";

        public static string ConvertMp4ToMp3(string pathOfMp4)
        {
            var copyOfpathOfMp4 = pathOfMp4;
            var pathToWav = ConvertToWav(pathOfMp4);
            var pathToMp3 = pathToWav.Replace(wav, mp3);

            ConvertWavToMp3(pathToWav, pathToMp3);

            DeleteFile(copyOfpathOfMp4);
            DeleteFile(pathToWav);

            return pathToMp3;
        }

        private static void DeleteFile(string filePath)
        {
            try
            {
                Log.Debug("Deleting file: '{0}'", filePath);
                File.Delete(filePath);
                Log.Debug("Deleted file: '{0}'", filePath);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("File '{0}' couldn't be deleted", filePath), ex);
            }
        }

        public static string ConvertToWav(string fromPath)
        {
            if (!File.Exists(fromPath))
            {
                Log.Error(string.Format("File '{0}' doesn't exist", fromPath), new Exception());
            }

            Log.Debug("Starting: '{0}' to: '{1}' conversion from file: '{1}'", m4a, wav, fromPath);

            var command = PathToExe + @"\faad.exe";
            var args = InQuotes(fromPath);

            var process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = args;

            Log.Debug("Command to run: '{0}' with args: '{1}'", command, args);

            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Failed '{0}' to '{1}' conversion from: '{2}'", m4a, wav, fromPath), ex);
            }
            finally
            {
                process.Dispose();
            }

            Log.Debug("Completed '{0}' to '{1}' conversion from: '{2}'", m4a, wav, fromPath);

            return fromPath.Replace(m4a, wav);
        }

        public static void ConvertWavToMp3(string fromPathWav, string toPathMp3)
        {
            if (!File.Exists(fromPathWav))
            {
                Log.Error(string.Format("File '{0}' doesn't exist", fromPathWav), new Exception());
            }

            Log.Debug("Starting conversion to: '{0}' from file: '{1}' to: '{2}'", mp3, fromPathWav, toPathMp3);

            var command = PathToExe + @"\lame.exe ";            
            var args = string.Format(" -V2 --cbr -b 192 {0} {1}", InQuotes(fromPathWav), InQuotes(toPathMp3));

            Log.Debug("Command to run: '{0}' with args: '{1}'", command, args);

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
                Log.Error(string.Format("Failed conversion to: '{0}' from: '{1}' to: '{2}'", mp3, fromPathWav, toPathMp3), ex);
            }
            finally
            {
                process.Dispose();
            }

            Log.Debug("Completed conversion to '{0}' from: '{1}' to: '{2}'", mp3, fromPathWav, toPathMp3);
        }

        private static string InQuotes(string withoutQuotes)
        {
            return "\"" + withoutQuotes + "\"";
        }
    }
}
