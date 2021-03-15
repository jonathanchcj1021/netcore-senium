using System;
using log4net;

namespace NetCoreSelenium.Library
{
    public class LogHelper
    {
        public static ILog log;
        public LogHelper(Type type)
        {
            log = LogManager.GetLogger(type);
        }

        public void Debug(string message)
        {
            log.Debug(message);
        }

        public void Debug(string message, Exception ex)
        {
            log.Debug(message, ex);
        }

        public void Error(string message)
        {
            log.Error(message);
        }

        public void Error(string message, Exception ex)
        {
            log.Error(message, ex);
        }

        public void Fatal(string message)
        {
            log.Fatal(message);
        }

        public void Fatal(string message, Exception ex)
        {
            log.Fatal(message, ex);
        }

        public void Info(string message)
        {
            log.Info(message);
        }

        public void Info(string message, Exception ex)
        {
            log.Info(message, ex);
        }

        public void Warn(string message)
        {
            log.Error(message);
        }

        public void Warn(string message, Exception ex)
        {
            log.Error(message, ex);
        }
    }

}
