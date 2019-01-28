using ReactiveUI;

namespace Whisper.Apps.Desktop.Views
{
    public partial class CreateItemView
    {
        public CreateItemView()
        {
            InitializeComponent();
            
            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, x => x.AvailableFactoryConfigurations, x => x.FactorySelector.ItemsSource));
                d(this.Bind(ViewModel, x => x.SelectedGeneratorConfiguration, x => x.FactorySelector.SelectedItem));
                d(this.BindCommand(ViewModel, x => x.CreateSelectedItemCommand, x => x.CreateItemButton));
            });
        }
    }
}
