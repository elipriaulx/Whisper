using System.Reactive.Disposables;
using ReactiveUI;

namespace Whisper.Apps.Desktop.Windows.Settings.Views
{
    public partial class SettingsPageGenerationView
    {
        public SettingsPageGenerationView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, x => x.AddConfigurationFromSelectedGeneratorCommand, x => x.GeneratorSelectorCreateButton).DisposeWith(d);
                this.BindCommand(ViewModel, x => x.RemoveSelectedConfigurationCommand, x => x.RemoveSelectedConfigurationButton).DisposeWith(d);
                this.BindCommand(ViewModel, x => x.MoveDownSelectedConfigurationCommand, x => x.MoveDownSelectedConfigurationButton).DisposeWith(d);
                this.BindCommand(ViewModel, x => x.MoveUpSelectedConfigurationCommand, x => x.MoveUpSelectedConfigurationButton).DisposeWith(d);
                
                this.OneWayBind(ViewModel, x => x.Generators, x => x.GeneratorSelectorComboBox.ItemsSource).DisposeWith(d);
                this.Bind(ViewModel, x => x.SelectedGenerator, x => x.GeneratorSelectorComboBox.SelectedItem).DisposeWith(d);
                
                this.OneWayBind(ViewModel, x => x.Configurations, x => x.ConfigurationSelectorListBox.ItemsSource).DisposeWith(d);
                this.Bind(ViewModel, x => x.SelectedConfiguration, x => x.ConfigurationSelectorListBox.SelectedItem).DisposeWith(d);

                this.OneWayBind(ViewModel, x => x.SelectedConfiguration, x => x.SelectedConfigurationHost.ViewModel).DisposeWith(d);
            });
        }
    }
}
