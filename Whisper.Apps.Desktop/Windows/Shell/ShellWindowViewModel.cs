using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Whisper.Apps.Desktop.ViewModels;
using Whisper.Apps.Desktop.Windows.Settings;
using Whisper.Apps.Desktop.Windows.Shell.Models;
using Whisper.Core.Models.Configuration;
using Whisper.Core.Services;

namespace Whisper.Apps.Desktop.Windows.Shell
{
    public class ShellWindowViewModel : ReactiveObject, IDisposable
    {
        private readonly IDisposable subscriptions;
        private readonly IConfigurationAgent<ShellConfiguration> configurationAgent;
        private readonly SettingsWindowManager _settingsManager;

        private bool _refreshingConfig;

        public ShellWindowViewModel(IConfigurationService configurationService, CreateItemViewModel creator, HistoryListViewModel historyList, SettingsWindowManager settingsManager)
        {
            _settingsManager = settingsManager;
            configurationAgent = configurationService.GetConfigurationAgent<ShellConfiguration>(Guid.Parse("64A5577D-5985-4C91-AE81-7D1E6AD8C969"), this);

            subscriptions = configurationAgent.Updated.ObserveOnDispatcher().Do(x => { RefreshConfig(); }).Subscribe();

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
        
        public void Dispose()
        {
            subscriptions?.Dispose();
        }

        private void RefreshConfig()
        {
            if (_refreshingConfig)
                return;

            try
            {
                _refreshingConfig = true;

                var configuration = configurationAgent.Get();

                AlwaysOnTop = configuration.AlwaysOnTop;
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

            var configuration = configurationAgent.Get();

            configuration.AlwaysOnTop = AlwaysOnTop;

            configurationAgent.Update(configuration);
        }
    }
}
