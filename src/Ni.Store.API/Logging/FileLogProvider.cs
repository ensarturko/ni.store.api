using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ni.Store.Api.Logging
{
    public class FileLogProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string category)
        {
            return new FileLogger();
        }
        public void Dispose()
        {

        }
    }
}
