using System;

namespace DKRUpdater.Core.Logging
{
    public class DKRlogger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Log(string message)
        {
            log.Debug(message);
        }

        public static void Debug(string format,params object[] args)
        {
            log.Debug(string.Format(format, args));
        }

        public static void LogError(string message, Exception ex)
        {
            log.Error(message, ex);
        }
    }
}
