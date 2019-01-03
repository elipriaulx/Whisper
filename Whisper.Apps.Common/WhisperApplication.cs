using System;
using System.IO;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Whisper.Apps.Common.Models.Initialisation;

namespace Whisper.Apps.Common
{
    public class WhisperApplication : IDisposable
    {
        public static readonly string LocalAppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Whisper");
        public static readonly string LoggingDirectory = Path.Combine(LocalAppDataDirectory, "Logs");
        public static readonly string ConfigDirectory = Path.Combine(LocalAppDataDirectory, "Config");

        public IObservable<InitialisationUpdate> InitialisationProgress => _initialisationProgress;

        private readonly Subject<InitialisationUpdate> _initialisationProgress = new Subject<InitialisationUpdate>();

        private readonly object _initialisationLock = new object();
        private bool _initialisationRequested = false;

        public void InitialiseApplication()
        {
            lock (_initialisationLock)
            {
                if (_initialisationRequested)
                    return;

                _initialisationRequested = true;
            }


        }


        public void Dispose()
        {
            _initialisationProgress?.Dispose();
        }

        private void RaiseUpdate(InitialisationStages stage, double progress = 0)
        {
            _initialisationProgress.OnNext(new InitialisationUpdate(stage, progress));
        }
    }
}
