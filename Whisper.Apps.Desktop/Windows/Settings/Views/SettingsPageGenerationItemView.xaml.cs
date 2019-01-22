using System.Reactive.Disposables;
using ReactiveUI;

namespace Whisper.Apps.Desktop.Windows.Settings.Views
{
    public partial class SettingsPageGenerationItemView
    {
        public SettingsPageGenerationItemView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.ViewModel).BindTo(ConfigurationMetaHost, x => x.Content).DisposeWith(d);
                //this.OneWayBind(ViewModel, x => x, x => x.ConfigurationMetaHost.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, x => x.Configuration, x => x.ConfigurationHost.Content).DisposeWith(d);
            });
        }
    }
}
