using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ni.Store.Api.Logging
{
    public class FileLogger : ILogger
    {
        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
            await WriteMessageToFile(message);
        }

        private static async Task WriteMessageToFile(string message)
        {
            const string filePath = "AspCoreFileLog.txt";

            using (var streamWriter = new StreamWriter(filePath, true))
            {
                await streamWriter.WriteLineAsync(message);
                streamWriter.Close();
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }
    }
}
