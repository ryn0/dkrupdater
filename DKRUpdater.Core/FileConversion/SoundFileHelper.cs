using DKRUpdater.Core.Interfaces;
using System.IO;
using System;
using DKRUpdater.Core.Constants;
using DKRUpdater.Core.Logging;
using System.Diagnostics;
using DKRUpdater.Core.FileSystem;

namespace DKRUpdater.Core.FileConversion
{
    public class SoundFileHelper : ISoundFileHelper
    {
        private static string TrimmedPathPrefix = "_TRIMMED";
        private static int MaxMp3Bitrate = 192;        
        private static string PathToExe = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Utilities";

        private int _currentBitRate = 0;

        public string ToTrimmedMp3(string pathToFile)
        {
            try
            {
                if (!File.Exists(pathToFile))
                {
                    Log.Error("File missing: '" + pathToFile + "'", new Exception());
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Log.Error("File error: '" + pathToFile + "'", ex);
                return string.Empty;
            }

            if (IsFileWav(pathToFile))
                return WavToTrimmedMp3(pathToFile);

            if (IsFileM4a(pathToFile))
                return M4aToTrimmedMp3(pathToFile);

            if (IsFileMp3(pathToFile))
                return Mp3ToTrimmedMp3(pathToFile);

            Log.Error("Unknown file to remove silence from: " + pathToFile, new Exception());

            return pathToFile;
        }

        private bool IsFileMp3(string pathToFile)
        {
            var extension = Path.GetExtension(pathToFile);

            return extension.ToLower() == StringConstants.mp3;
        }

        private bool IsFileWav(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            return extension.ToLower() == StringConstants.wav;
        }

        public bool IsFileM4a(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            return extension.ToLower() == StringConstants.m4a;
        }

        private string ConvertWavToMp3(string trimmedWavPath)
        {
            var pathToMp3 = trimmedWavPath.Replace(StringConstants.wav, StringConstants.mp3);

            ConvertWavToMp3(trimmedWavPath, pathToMp3);

            FileOperations.DeleteFile(trimmedWavPath);

            return pathToMp3;
        }

        private string WavToTrimmedMp3(string pathToWav)
        {
            var trimmedWavPath = TrimWav(pathToWav);
            var mp3Path = ConvertWavToMp3(trimmedWavPath);

            FileOperations.DeleteFile(trimmedWavPath);

            return mp3Path;
        }

        private string Mp3ToTrimmedMp3(string pathToMp3)
        {
            _currentBitRate = BitRateInMp3(pathToMp3);
            var pathToWav = ConvertMp3ToWav(pathToMp3);
            var trimmedWavPath = TrimWav(pathToWav);
            var trimmedMp3Path = ConvertWavToMp3(trimmedWavPath);

            return trimmedMp3Path;
        }

        private string M4aToTrimmedMp3(string pathToFile)
        {
            var pathToWav = ConvertM4aToWav(pathToFile);

            return WavToTrimmedMp3(pathToWav);
        }

        private string ConvertMp3ToWav(string pathToMp3)
        {
            var pathToWav = pathToMp3.Replace(StringConstants.mp3, StringConstants.wav);

            if (!File.Exists(pathToMp3))
            {
                Log.Error(string.Format("File '{0}' doesn't exist", pathToMp3), new Exception());
            }

            Log.Debug("Starting: '{0}' to: '{1}' conversion from file: '{2}' to: '{3}'...", StringConstants.mp3, StringConstants.wav, pathToMp3, pathToWav);

            var command = PathToExe + @"\ffmpeg.exe";
            var args = string.Format(@" -i {0} {1}", InQuotes(pathToMp3), InQuotes(pathToWav));

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
                Log.Error(string.Format("Failed '{0}' to '{1}' conversion from: '{2}'", StringConstants.mp3, StringConstants.wav, pathToMp3), ex);
                return string.Empty;
            }
            finally
            {
                process.Dispose();
            }

            Log.Debug("Completed conversion.");

            FileOperations.DeleteFile(pathToMp3);

            return pathToWav;
        }

        private string TrimWav(string pathToWav)
        {
            if (!File.Exists(pathToWav))
            {
                Log.Error(string.Format("File '{0}' doesn't exist", pathToWav), new Exception());

                return string.Empty;
            }

            var trimmedWavPath = pathToWav.Replace(StringConstants.wav, TrimmedPathPrefix + StringConstants.wav);

            Log.Debug("Trimming: '{0}' to: '{1}'...", pathToWav, trimmedWavPath);

            var command = PathToExe + @"\sox-14-4-2\sox.exe";
            var args = string.Format(" {0} {1} silence 1 0.1 1% reverse silence 1 0.1 1% reverse ", InQuotes(pathToWav), InQuotes(trimmedWavPath));
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
                Log.Error("Failed to trim file", ex);
                return string.Empty;
            }
            finally
            {
                process.Dispose();
            }

            Log.Debug("Completed trim.");

            FileOperations.DeleteFile(pathToWav);

            FileOperations.RenameFile(trimmedWavPath, pathToWav);

            trimmedWavPath = pathToWav;

            return trimmedWavPath;
        }

        private string ConvertM4aToWav(string pathToM4a)
        {
            if (!File.Exists(pathToM4a))
            {
                Log.Error(string.Format("File '{0}' doesn't exist", pathToM4a), new Exception());
                return string.Empty;
            }

            Log.Debug("Starting: '{0}' to: '{1}' conversion from file: '{2}'", StringConstants.m4a, StringConstants.wav, pathToM4a);

            var command = PathToExe + @"\faad.exe";
            var args = InQuotes(pathToM4a);

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
                Log.Error(string.Format("Failed '{0}' to '{1}' conversion from: '{2}'", StringConstants.m4a, StringConstants.wav, pathToM4a), ex);
                return string.Empty;
            }
            finally
            {
                process.Dispose();
            }

            Log.Debug("Completed '{0}' to '{1}' conversion from: '{2}'", StringConstants.m4a, StringConstants.wav, pathToM4a);

            FileOperations.DeleteFile(pathToM4a);

            return pathToM4a.Replace(StringConstants.m4a, StringConstants.wav);
        }


        public int BitRateInMp3(string path)
        {
            var mp3Track = TagLib.File.Create(path);

            return mp3Track.Properties.AudioBitrate;
        }
    
        private void ConvertWavToMp3(string fromPathWav, string toPathMp3)
        {
            if (!File.Exists(fromPathWav))
            {
                Log.Error(string.Format("File '{0}' doesn't exist", fromPathWav), new Exception());
                return;
            }

            Log.Debug("Starting conversion to: '{0}' from file: '{1}' to: '{2}'", StringConstants.mp3, fromPathWav, toPathMp3);

            var bitRateToUse = _currentBitRate == 0 || _currentBitRate >= MaxMp3Bitrate ?
                               MaxMp3Bitrate :
                               _currentBitRate;

            var command = PathToExe + @"\lame.exe ";
            var args = string.Format(" -V2 --cbr -b {0} {1} {2}", bitRateToUse, InQuotes(fromPathWav), InQuotes(toPathMp3));

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
                Log.Error(string.Format("Failed conversion to: '{0}' from: '{1}' to: '{2}'", StringConstants.mp3, fromPathWav, toPathMp3), ex);
                return;
            }
            finally
            {
                process.Dispose();
            }

            Log.Debug("Completed conversion to '{0}' from: '{1}' to: '{2}'", StringConstants.mp3, fromPathWav, toPathMp3);
        }

        private static string InQuotes(string withoutQuotes)
        {
            return "\"" + withoutQuotes + "\"";
        }
    }
}
