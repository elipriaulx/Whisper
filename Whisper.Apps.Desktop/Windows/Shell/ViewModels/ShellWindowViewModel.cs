using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Whisper.Apps.Desktop.Models.Configurations;
using Whisper.Apps.Desktop.ViewModels;
using Whisper.Apps.Desktop.Windows.Settings;
using Whisper.Core.Models.Configuration;
using Whisper.Core.Services;

namespace Whisper.Apps.Desktop.Windows.Shell
{
    public class ShellWindowViewModel : ReactiveObject, IDisposable
    {
        private readonly IDisposable _subscriptions;
        private readonly IConfigurationAgent<ShellConfiguration> _configurationAgent;
        private readonly SettingsWindowManager _settingsManager;

        private bool _refreshingConfig;

        public ShellWindowViewModel(IConfigurationService configurationService, CreateItemViewModel creator, HistoryListViewModel historyList, SettingsWindowManager settingsManager)
        {
            _settingsManager = settingsManager;
            _configurationAgent = configurationService.GetConfigurationAgent<ShellConfiguration>(ApplicationConfigurations.ShellConfiguration, this);

            _subscriptions = _configurationAgent.Updated.ObserveOnDispatcher().Do(x => { RefreshConfig(); }).Subscribe();

            Creator = creator;
            HistoryList = historyList;

            ShowSettingsCommand = ReactiveCommand.Create(() => { _settingsManager.ShowSettingsWindow(); });

            RefreshConfig();

            PropertyChanged += (sender, e) => { PersistConfig(); };
        }

        public CreateItemViewModel Creator { get; }

        public HistoryListViewModel HistoryList { get; }

        public ICommand ShowSettingsCommand { get; }

        [Reactive]
        public bool AlwaysOnTop { get; set; }

        [Reactive]
        public bool AllowMinimiseToTray { get; private set; }
        
        public void Dispose()
        {
            _subscriptions?.Dispose();
        }

        private void RefreshConfig()
        {
            if (_refreshingConfig)
                return;

            try
            {
                _refreshingConfig = true;

                var configuration = _configurationAgent.Get();

                AlwaysOnTop = configuration.EnableAlwaysOnTop;
                AllowMinimiseToTray = configuration.EnableMinimiseToTray;
            }
            finally
            {
                _refreshingConfig = false;
            }
        }

        private void PersistConfig()
        {
            if (_refreshingConfig)
                return;

            var configuration = _configurationAgent.Get();

            configuration.EnableAlwaysOnTop = AlwaysOnTop;
            configuration.EnableMinimiseToTray = AllowMinimiseToTray;

            _configurationAgent.Update(configuration);
        }
    }
}
