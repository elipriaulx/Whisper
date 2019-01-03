using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Whisper.Core.Services;

namespace Whisper.Apps.Desktop.ViewModels
{
    public class CreateItemViewModel : ReactiveObject
    {
        private readonly IGeneratorService _generator;
        private readonly IClipboardService _clipboardService;
        private readonly List<CreateItemFactoryConfigurationViewModel> _availableFactoryConfigurations = new List<CreateItemFactoryConfigurationViewModel>();

        public CreateItemViewModel(IGeneratorService generator, IClipboardService clipboardService)
        {
            _generator = generator;
            _clipboardService = clipboardService;

            _availableFactoryConfigurations = _generator.FactoryInfo.Values.Select(x => new CreateItemFactoryConfigurationViewModel(x, _generator)).ToList();
            AvailableFactoryConfigurations = _availableFactoryConfigurations;
            SelectedFactoryConfiguration = AvailableFactoryConfigurations?.FirstOrDefault();

            CreateSelectedItemCommand = ReactiveCommand.Create(() =>
            {
                var instance = SelectedFactoryConfiguration?.CreateInstance();
                instance?.SetToClipboard(_clipboardService);
            });
        }

        [Reactive]
        public IEnumerable<CreateItemFactoryConfigurationViewModel> AvailableFactoryConfigurations { get; set; }

        [Reactive]
        public CreateItemFactoryConfigurationViewModel SelectedFactoryConfiguration { get; set; }

        public ICommand CreateSelectedItemCommand { get; }
    }
}
