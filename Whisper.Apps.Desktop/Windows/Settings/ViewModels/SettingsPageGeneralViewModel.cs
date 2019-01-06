using ReactiveUI.Fody.Helpers;
using Whisper.Apps.Common.Models.Configurations;
using Whisper.Apps.Desktop.Models.Configurations;
using Whisper.Core.Models.Configuration;
using Whisper.Core.Services;

namespace Whisper.Apps.Desktop.Windows.Settings.ViewModels
{
    public class SettingsPageGeneralViewModel : SettingsPageViewModelBase
    {
        private readonly IConfigurationAgent<ShellConfiguration> _shellConfigurationAgent;
        private readonly IConfigurationAgent<FactoryMonitorConfiguration> _factoryMonitorConfigurationAgent;

        public SettingsPageGeneralViewModel(IConfigurationService configService)
        {
            _shellConfigurationAgent = configService.GetConfigurationAgent<ShellConfiguration>(ApplicationConfigurations.ShellConfiguration, this);
            _factoryMonitorConfigurationAgent = configService.GetConfigurationAgent<FactoryMonitorConfiguration>(CommonConfigurations.FactoryMonitorConfiguration, this);
        }

        public override string Name => "General";
        public override string Description => "General Settings.";

        [Reactive]
        public bool EnableAlwaysOnTop { get; set; }

        [Reactive]
        public bool EnableMinimiseToTray { get; set; }

        [Reactive]
        public bool EnableAutoCopyToClipboard { get; set; }
        
        public override void ApplyConfigChanges()
        {
            var shellConfig = _shellConfigurationAgent.Get();
            var factoryMonitorConfig = _factoryMonitorConfigurationAgent.Get();

            shellConfig.EnableMinimiseToTray = EnableMinimiseToTray;
            shellConfig.EnableAlwaysOnTop = EnableAlwaysOnTop;
            factoryMonitorConfig.EnableAutoCopy = EnableAutoCopyToClipboard;

            _shellConfigurationAgent.Update(shellConfig);
            _factoryMonitorConfigurationAgent.Update(factoryMonitorConfig);
        }

        public override void CancelConfigChanges()
        {
            var shellConfig = _shellConfigurationAgent.Get();
            var factoryMonitorConfig = _factoryMonitorConfigurationAgent.Get();

            EnableMinimiseToTray = shellConfig.EnableMinimiseToTray;
            EnableAlwaysOnTop = shellConfig.EnableAlwaysOnTop;
            EnableAutoCopyToClipboard = factoryMonitorConfig.EnableAutoCopy;
        }
    }
}
