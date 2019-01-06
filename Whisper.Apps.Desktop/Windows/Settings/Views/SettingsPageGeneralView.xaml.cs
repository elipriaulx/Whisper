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
                this.Bind(ViewModel, x => x.EnableAlwaysOnTop, x => x.AlwaysOnTopCheckBox.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, x => x.EnableMinimiseToTray, x => x.MinimiseToTrayCheckBox.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, x => x.EnableAutoCopyToClipboard, x => x.AutoCopyCheckBox.IsChecked).DisposeWith(d);
            });
        }
    }
}
