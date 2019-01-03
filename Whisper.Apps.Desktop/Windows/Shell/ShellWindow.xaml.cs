using System.Reactive.Disposables;
using ReactiveUI;

namespace Whisper.Apps.Desktop.Windows.Shell
{
    public partial class ShellWindow : IViewFor<ShellWindowViewModel>
    {
        public ShellWindow()
        {
            InitializeComponent();
            
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Creator, x => x.CreatorHost.ViewModel).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.HistoryList, x => x.HistoryListHost.ViewModel).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.AlwaysOnTop, x => x.Topmost).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.ShowSettingsCommand, x => x.SettingsButton).DisposeWith(disposables);
            });
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set
            {
                ViewModel = (ShellWindowViewModel) value;
                DataContext = value;
            }
        }

        public ShellWindowViewModel ViewModel { get; set; }
    }
}
