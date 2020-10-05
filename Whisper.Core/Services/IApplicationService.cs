using System;

namespace Whisper.Core.Services
{
    public interface IApplicationService : IDisposable
    {
        void Start();
        void Stop();
    }
}
