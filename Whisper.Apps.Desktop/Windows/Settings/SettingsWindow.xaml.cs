using System.Reactive.Disposables;
using ReactiveUI;

namespace Whisper.Apps.Desktop.Windows.Settings
{
    public partial class SettingsWindow : IViewFor<SettingsWindowViewModel>
    {
        private SettingsWindowViewModel _viewModel;

        public SettingsWindow()
        {
            InitializeComponent();

            // Don't use ReactiveUI Bindings here, it fucks up the default DataTemplate resolution with its AutoDataTemplateBindingHook

            //this.WhenActivated(disposables =>
            //{
            //    this.OneWayBind(ViewModel, x => x.CurrentPage, x => x.ContentHost.ViewModel).DisposeWith(disposables);
            //    this.OneWayBind(ViewModel, x => x.Pages, x => x.ContentSelector.ItemsSource).DisposeWith(disposables);
            //    this.Bind(ViewModel, x => x.CurrentPage, x => x.ContentSelector.SelectedItem).DisposeWith(disposables);
            //});
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (SettingsWindowViewModel)value;
        }

        public SettingsWindowViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                DataContext = value;
            }
        }
    }
}
