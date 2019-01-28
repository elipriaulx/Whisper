using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Whisper.Apps.Common.Models.Configurations;
using Whisper.Core.Models.Configuration;
using Whisper.Core.Services;

namespace Whisper.Apps.Desktop.ViewModels
{
    public class CreateItemViewModel : ReactiveObject
    {
        private readonly IGeneratorService _generator;
        private readonly IClipboardService _clipboardService;

        private readonly IConfigurationAgent<FactoryMonitorConfiguration> _factoryMonitorConfigurationAgent;
        private readonly IConfigurationAgent<GeneratorConfiguration> _generatorConfigurations;

        private FactoryMonitorConfiguration _factoryMonitorConfig;
        
        public CreateItemViewModel(IConfigurationService configService, IGeneratorService generator, IClipboardService clipboardService)
        {
            _generator = generator;
            _clipboardService = clipboardService;

            _factoryMonitorConfigurationAgent = configService.GetConfigurationAgent<FactoryMonitorConfiguration>(CommonConfigurations.FactoryMonitorConfiguration, this);
            _factoryMonitorConfigurationAgent.Updated.Do(x => _factoryMonitorConfig = _factoryMonitorConfigurationAgent.Get()).Subscribe();
            _factoryMonitorConfig = _factoryMonitorConfigurationAgent.Get();

            _generatorConfigurations = configService.GetConfigurationAgent<GeneratorConfiguration>(CommonConfigurations.GeneratorConfiguration, this);
            _generatorConfigurations.Updated.ObserveOnDispatcher().Do(x => ConvertGeneratorConfigurations()).Subscribe();
            ConvertGeneratorConfigurations();
            
            CreateSelectedItemCommand = ReactiveCommand.Create(() =>
            {
                var instance = SelectedGeneratorConfiguration?.CreateInstance();

                if (_factoryMonitorConfig.EnableAutoCopy)
                    instance?.SetToClipboard(_clipboardService);
            });
        }

        private void ConvertGeneratorConfigurations()
        {
            var config = _generatorConfigurations.Get();

            var configItems = config.Items.Where(x => _generator.GeneratorInfo.ContainsKey(x.GeneratorId)).Select(c =>
            {
                var g = _generator.GeneratorInfo[c.GeneratorId];
                var configItem = g.DeserialiseConfiguration(c.ConfigurationStructure);

                return new CreateItemGeneratorConfigurationViewModel(c.Id, g.Id, g.Name, c.Name, c.Description, configItem, _generator);
            }).ToList();

            AvailableFactoryConfigurations = configItems;
            SelectedGeneratorConfiguration = configItems.FirstOrDefault();
        }

        [Reactive]
        public IEnumerable<CreateItemGeneratorConfigurationViewModel> AvailableFactoryConfigurations { get; set; }

        [Reactive]
        public CreateItemGeneratorConfigurationViewModel SelectedGeneratorConfiguration { get; set; }

        public ICommand CreateSelectedItemCommand { get; }
    }
}
