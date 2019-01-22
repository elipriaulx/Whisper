using System.Reactive.Disposables;
using ReactiveUI;

namespace Whisper.Apps.Desktop.Windows.Settings.Views
{
    public partial class SettingsPageGeneralView
    {
        public SettingsPageGeneralView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, x => x.ShellConfig, x => x.ShellConfigurationHost.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, x => x.FactoryMonitorConfig, x => x.FactoryMonitorConfigurationHost.Content).DisposeWith(d);
            });
        }
    }
}
