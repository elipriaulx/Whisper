using Whisper.Core.Models.Logging;

namespace Whisper.Core.Services
{
    public interface ILoggingService
    {
        ILogger GetDefaultLogger();
        IDisposableLogger GetContextualLogger(string name);
    }
}