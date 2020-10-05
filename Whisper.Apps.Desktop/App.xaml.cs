using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
using Whisper.Apps.Desktop.Windows.Splash;
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

        // TODO: Review hot-key situation
        //https://stackoverflow.com/questions/28785375/c-sharp-wpf-catch-keydown-even-when-minimized
        // https://www.dreamincode.net/forums/topic/180436-global-hotkeys/

        private WhisperApplication _applicationInstance;

        protected override async void OnStartup(StartupEventArgs e)
        {
            _applicationInstance = new WhisperApplication();

            // Initialise the application instance.
            using (var splash = new SplashWindow(_applicationInstance.InitialisationProgress))
            {
                splash.Show();

                var timer = System.Diagnostics.Stopwatch.StartNew();
                await _applicationInstance.InitialiseApplication();
                timer.Stop();

                var delayDelta = (int)(1000 - timer.ElapsedMilliseconds);

                if (delayDelta > 0)
                    await Task.Delay(delayDelta);


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
                    new SettingsPageAboutViewModel(_applicationInstance.AppInfoService),
                    new SettingsPageGeneralViewModel(_applicationInstance.ConfigService),
                    //new SettingsPageApplicationViewModel(),
                    new SettingsPageGenerationViewModel(_applicationInstance.ConfigService, _applicationInstance.GeneratorService)
                    });

                    settingsWindow.ViewModel = settingsVm;

                    return settingsWindow;
                };

                var settingsManager = new SettingsWindowManager(settingsWindowFactory);

                var shellWindowViewModel = new ShellWindowViewModel(_applicationInstance.ConfigService, new CreateItemViewModel(_applicationInstance.ConfigService, _applicationInstance.GeneratorService, _applicationInstance.ClipboardService), new HistoryListViewModel(_applicationInstance.GeneratorService, _applicationInstance.ClipboardService), settingsManager);

                // Fix this bat-shit nonsense.
                Locator.CurrentMutable.UnregisterAll<IPropertyBindingHook>();
                Locator.CurrentMutable.Register<IPropertyBindingHook>(() => new BindingHookFixerer());

                var shell = new ShellWindow
                {
                    ViewModel = shellWindowViewModel
                };

                var trayIcon = new WhisperTrayAgent(shell, settingsManager);
                _applicationDisposables.Add(trayIcon);

                splash.Hide();
                splash.Close();

                shell.Show();
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _applicationDisposables.Dispose();
            _applicationInstance?.Dispose();

            base.OnExit(e);
        }
    }
}
