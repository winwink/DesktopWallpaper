using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Winwink.DesktopWallPaper
{
    public class LogWriter
    {
        private static object _lockObject = new object();
        private static string _logDir;
        static LogWriter()
        {
            _logDir = AppDomain.CurrentDomain.BaseDirectory + "\\Log";
            if (!Directory.Exists(_logDir))
            {
                Directory.CreateDirectory(_logDir);
            }
        }
        public static void Write(string source, string message, LogLevel logLevel = LogLevel.ERROR)
        {
            string path = Path.Combine(_logDir, DateTime.Now.ToString("yyyyMM") + ".log");
            WriteImpl(source, message, path, logLevel);
        }

        public void Write(string source, string message, Dictionary<string, object> variables, LogLevel logLevel = LogLevel.ERROR)
        {
            var variableMsg = string.Join(";", variables.Select(m => string.Format("[{0}]:{1}", m.Key, m.Value)));
            message = message + ",[VAR]:" + variableMsg;
            Write(source, message, logLevel);
        }

        private static void WriteImpl(string source, string message, string logPath, LogLevel logLevel)
        {
            lock (_lockObject)
            {
                using (FileStream fileStream = new FileStream(logPath, FileMode.Append))
                {
                    StreamWriter sw = new StreamWriter(fileStream);
                    sw.WriteLine("{0}\t{1}\t{2},{3}", DateTime.Now.ToString("HH:mm:ss"), logLevel.ToString(), source, message);
                    sw.Close();
                }
            }
        }
    }

    public enum LogLevel
    {
        ALL = 0,
        DEBUG = 1,
        INFO = 2,
        ERROR = 3,
        NONE = 4
    }
}
