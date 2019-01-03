using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI;
using Splat;
using Whisper.Apps.Common;
using Whisper.Apps.Common.Services;
using Whisper.Apps.Desktop.Obsolete;
using Whisper.Apps.Desktop.TrayAgent;
using Whisper.Apps.Desktop.ViewModels;
using Whisper.Apps.Desktop.Views;
using Whisper.Apps.Desktop.Windows.Settings;
using Whisper.Apps.Desktop.Windows.Shell;

namespace Whisper.Apps.Desktop
{
    public partial class App : Application
    {
        //https://stackoverflow.com/questions/28785375/c-sharp-wpf-catch-keydown-even-when-minimized
        // https://www.dreamincode.net/forums/topic/180436-global-hotkeys/

        WhisperApplication _applicationInstance;

        private readonly CompositeDisposable _applicationDisposables = new CompositeDisposable();

        protected override void OnStartup(StartupEventArgs e)
        {
            _applicationInstance = new WhisperApplication();

            var configPath = Path.Combine(WhisperApplication.ConfigDirectory, "application.config");

            if (!Directory.Exists(WhisperApplication.LoggingDirectory))
                Directory.CreateDirectory(WhisperApplication.LoggingDirectory);
            if (!Directory.Exists(WhisperApplication.ConfigDirectory))
                Directory.CreateDirectory(WhisperApplication.ConfigDirectory);

            var loggingService = new LoggingServiceProvider(WhisperApplication.LoggingDirectory);
            var clipboardService = new ClipboardServiceProvider();
            var configService = new ConfigurationServiceProvider(loggingService);

            configService.LoadConfiguration(configPath);

            _applicationDisposables.Add(configService.Updated.Throttle(TimeSpan.FromMilliseconds(500)).Do(x => { configService.SaveConfiguration(configPath); }).Subscribe());

            var generatorService = new GeneratorServiceProvider();
            
            generatorService.AddFactory(new GuidInstanceFactory());
            generatorService.AddFactory(new PasswordInstanceFactory());
            
            Locator.CurrentMutable.Register(() => new CreateItemView(), typeof(IViewFor<CreateItemViewModel>));
            Locator.CurrentMutable.Register(() => new HistoryListItemView(), typeof(IViewFor<HistoryListItemViewModel>));
            Locator.CurrentMutable.Register(() => new HistoryListView(), typeof(IViewFor<HistoryListViewModel>));

            var settingsManager = new SettingsWindowManager();

            var shellWindowViewModel = new ShellWindowViewModel(configService, new CreateItemViewModel(generatorService, clipboardService), new HistoryListViewModel(generatorService, clipboardService), settingsManager);
            
            var shell = new ShellWindow()
            {
                ViewModel = shellWindowViewModel
            };

            var noticon = new WhisperTrayAgent(shell, settingsManager);
            _applicationDisposables.Add(noticon);

            shell.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _applicationInstance?.Dispose();

            _applicationDisposables.Dispose();
            base.OnExit(e);
        }
    }
}
