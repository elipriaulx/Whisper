using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whisper.Core.Models.Logging;
using Whisper.Core.Services;

namespace Whisper.Apps.Common.Services
{
    public class LoggingServiceProvider : ILoggingService
    {
        private readonly Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();

        public LoggingServiceProvider(string loggingDirectory, string defaultLoggerName = "application")
        {

        }

        public ILogger GetDefaultLogger()
        {
            return null;
        }

        public IDisposableLogger GetContextualLogger(string name)
        {
            return null;
        }
    }
}
