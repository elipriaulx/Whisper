using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using Splat;
using Whisper.Apps.Common;
using Whisper.Apps.Common.Services;
using Whisper.Apps.Desktop.TrayAgent;
using Whisper.Apps.Desktop.ViewModels;
using Whisper.Apps.Desktop.Views;
using Whisper.Apps.Desktop.Windows.Settings;
using Whisper.Apps.Desktop.Windows.Settings.ViewModels;
using Whisper.Apps.Desktop.Windows.Settings.Views;
using Whisper.Apps.Desktop.Windows.Shell;
using Whisper.Modules.GuidGenerator;
using Whisper.Modules.NumberGenerator;
using Whisper.Modules.PasswordGenerator;

namespace Whisper.Apps.Desktop
{
    public partial class App : Application
    {
        public class BindingHookFixerer : IPropertyBindingHook
        {
            public bool ExecuteHook(object source, object target, Func<IObservedChange<object, object>[]> getCurrentViewModelProperties, Func<IObservedChange<object, object>[]> getCurrentViewProperties, BindingDirection direction)
            {
                return true;
            }
        }

        private readonly CompositeDisposable _applicationDisposables = new CompositeDisposable();
        //https://stackoverflow.com/questions/28785375/c-sharp-wpf-catch-keydown-even-when-minimized
        // https://www.dreamincode.net/forums/topic/180436-global-hotkeys/

        private WhisperApplication _applicationInstance;

        protected override void OnStartup(StartupEventArgs e)
        {
            _applicationInstance = new WhisperApplication();

            var configPath = Path.Combine(WhisperApplication.ConfigDirectory, "application.config");

            if (!Directory.Exists(WhisperApplication.LoggingDirectory))
                Directory.CreateDirectory(WhisperApplication.LoggingDirectory);
            if (!Directory.Exists(WhisperApplication.ConfigDirectory))
                Directory.CreateDirectory(WhisperApplication.ConfigDirectory);

            var appInfoService = new ApplicationInfoServiceProvider();
            var loggingService = new LoggingServiceProvider(WhisperApplication.LoggingDirectory);
            var clipboardService = new ClipboardServiceProvider();
            var configService = new ConfigurationServiceProvider(loggingService);

            configService.LoadConfiguration(configPath);

            _applicationDisposables.Add(configService.Updated.Throttle(TimeSpan.FromMilliseconds(500)).Do(x => { configService.SaveConfiguration(configPath); }).Subscribe());

            var generatorService = new GeneratorServiceProvider();

            generatorService.AddFactory(new GuidGenerator());
            generatorService.AddFactory(new PasswordGenerator());
            generatorService.AddFactory(new NumberGenerator());

            Locator.CurrentMutable.Register(() => new CreateItemView(), typeof(IViewFor<CreateItemViewModel>));
            Locator.CurrentMutable.Register(() => new HistoryListItemView(), typeof(IViewFor<HistoryListItemViewModel>));
            Locator.CurrentMutable.Register(() => new HistoryListView(), typeof(IViewFor<HistoryListViewModel>));

            Locator.CurrentMutable.Register(() => new SettingsPageAboutView(), typeof(IViewFor<SettingsPageAboutViewModel>));
            Locator.CurrentMutable.Register(() => new SettingsPageApplicationView(), typeof(IViewFor<SettingsPageApplicationViewModel>));
            Locator.CurrentMutable.Register(() => new SettingsPageGeneralView(), typeof(IViewFor<SettingsPageGeneralViewModel>));
            Locator.CurrentMutable.Register(() => new SettingsPageGenerationView(), typeof(IViewFor<SettingsPageGenerationViewModel>));
            Locator.CurrentMutable.Register(() => new SettingsPageGenerationItemView(), typeof(IViewFor<SettingsPageGenerationItemViewModel>));

            Func<SettingsWindow> settingsWindowFactory = () =>
            {
                var settingsWindow = new SettingsWindow();

                var settingsVm = new SettingsWindowViewModel(new List<SettingsPageViewModelBase>
                {
                    new SettingsPageAboutViewModel(appInfoService),
                    new SettingsPageGeneralViewModel(configService),
                    //new SettingsPageApplicationViewModel(),
                    new SettingsPageGenerationViewModel(configService, generatorService)
                });

                settingsWindow.ViewModel = settingsVm;

                return settingsWindow;
            };

            var settingsManager = new SettingsWindowManager(settingsWindowFactory);

            var shellWindowViewModel = new ShellWindowViewModel(configService, new CreateItemViewModel(configService, generatorService, clipboardService), new HistoryListViewModel(generatorService, clipboardService), settingsManager);

            // Fix this bat-shit nonsense.
            Locator.CurrentMutable.UnregisterAll<IPropertyBindingHook>();
            Locator.CurrentMutable.Register<IPropertyBindingHook>(() => new BindingHookFixerer());

            var shell = new ShellWindow
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
