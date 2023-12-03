using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Model
{
    public class LoggerProvider
    {
        private static readonly ILoggerFactory loggerFactory;

        static LoggerProvider()
        {
            string baseDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            // Create logger object
            loggerFactory = LoggerFactory.Create(builder =>
            {
                LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                    .WriteTo.File(baseDirectory + "\\Log\\Error\\Log.txt", rollingInterval: RollingInterval.Day);

                builder.AddSerilog(loggerConfiguration.CreateLogger());
            });
        }

        public static ILogger<T> CreateLogger<T>()
        {
            return loggerFactory.CreateLogger<T>();
        }
    }
}
