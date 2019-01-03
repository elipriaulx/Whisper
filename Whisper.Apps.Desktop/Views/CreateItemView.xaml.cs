using ReactiveUI;

namespace Whisper.Apps.Desktop.Views
{
    public partial class CreateItemView
    {
        public CreateItemView()
        {
            InitializeComponent();

            FactorySelector.DisplayMemberPath = "Name";

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, x => x.AvailableFactoryConfigurations, x => x.FactorySelector.ItemsSource));
                d(this.Bind(ViewModel, x => x.SelectedFactoryConfiguration, x => x.FactorySelector.SelectedItem));
                d(this.BindCommand(ViewModel, x => x.CreateSelectedItemCommand, x => x.CreateItemButton));
            });
        }
    }
}
