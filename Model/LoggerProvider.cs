using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Dictionary.Model
{
    public class LoggerProvider
    {
        private static readonly ILoggerFactory loggerFactory;

        static LoggerProvider()
        {
            string appDataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "En-Vi Dictionary");
            string logFolderPath = Path.Combine(appDataFolderPath, "Log");
            string errorLogFolderPath = Path.Combine(logFolderPath, "Error");
            string fullPath = Path.Combine(errorLogFolderPath, "Log.txt");

            loggerFactory = LoggerFactory.Create(builder =>
            {
                LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                    .WriteTo.File(fullPath, rollingInterval: RollingInterval.Day);
                builder.AddSerilog(loggerConfiguration.CreateLogger());
            });

        }

        public static ILogger<T> CreateLogger<T>()
        {
            return loggerFactory.CreateLogger<T>();
        }
    }
}
