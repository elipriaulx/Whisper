using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Whisper.Apps.Common;

namespace Whisper.Apps.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var application = new WhisperApplication();
            var subscriptions = new CompositeDisposable
            {
                application.InitialisationProgress.Do(x =>
                {
                    Console.WriteLine($"{x.Stage}: {x.Message}");
                }).Subscribe()
            };

            await application.InitialiseApplication();

            // TODO: Implement Interactive/non-interactive console version.

            Console.ReadLine();
        }
    }
}
