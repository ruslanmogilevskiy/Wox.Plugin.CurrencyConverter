using System;
using System.IO;
using System.Reflection;

namespace Wox.Plugin.CurrencyConverter.Logging
{
    static class Logger
    {
        static string GetLogFilePath()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, CurrencyConverterConstants.LogFileName);
        }

        public static void Log(string message)
        {
            try
            {
                File.AppendAllText(GetLogFilePath(), $"\r\n{DateTime.Now:G}: {message}");
            }
            catch
            {
            }
        }
    }
}