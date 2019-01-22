using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Whisper.Apps.Common.Models.Configurations;
using Whisper.Core.Models.Configuration;
using Whisper.Core.Services;

namespace Whisper.Apps.Desktop.Windows.Settings.ViewModels
{
    public class SettingsPageGenerationViewModel : SettingsPageViewModelBase
    {
        private readonly IGeneratorCatalogueService _generatorCatalogueService;
        private readonly IConfigurationAgent<GeneratorConfiguration> _generatorConfigurations;

        public SettingsPageGenerationViewModel(IConfigurationService configService, IGeneratorCatalogueService generatorCatalogueService)
        {
            _generatorCatalogueService = generatorCatalogueService;
            _generatorConfigurations = configService.GetConfigurationAgent<GeneratorConfiguration>(CommonConfigurations.GeneratorConfiguration, this);

            AddConfigurationFromSelectedGeneratorCommand = ReactiveCommand.Create(() =>
            {
                if (SelectedGenerator == null) return;

                var newConfig = SelectedGenerator.CreateConfigurationInstance();
                var vm = ViewModelFromLocalConfiguration(newConfig);

                Configurations.Add(vm);

                if (SelectedConfiguration == null)
                    SelectedConfiguration = Configurations.FirstOrDefault();
            });

            RemoveSelectedConfigurationCommand = ReactiveCommand.Create(() =>
            {
                if (SelectedConfiguration == null) return;

                var index = Configurations.IndexOf(SelectedConfiguration);

                Configurations.Remove(SelectedConfiguration);

                if (Configurations.Count > index && index >= 0)
                    SelectedConfiguration = Configurations[index];
                else
                    SelectedConfiguration = Configurations.LastOrDefault();
            });

            MoveDownSelectedConfigurationCommand = ReactiveCommand.Create(() =>
            {
                var index = Configurations.IndexOf(SelectedConfiguration);
                var targetIndex = index + 1;

                if (targetIndex >= Configurations.Count)
                    return;

                var s = SelectedConfiguration;
                Configurations.Remove(s);
                Configurations.Insert(targetIndex, s);
                SelectedConfiguration = s;
            });

            MoveUpSelectedConfigurationCommand = ReactiveCommand.Create(() =>
            {
                var index = Configurations.IndexOf(SelectedConfiguration);
                var targetIndex = index - 1;

                if (targetIndex < 0)
                    return;

                var s = SelectedConfiguration;
                Configurations.Remove(s);
                Configurations.Insert(targetIndex, s);
                SelectedConfiguration = s;
            });
        }

        public override string Name => "Generation";
        public override string Description => "Generation settings.";

        public ICommand AddConfigurationFromSelectedGeneratorCommand { get; }
        public ICommand RemoveSelectedConfigurationCommand { get; }
        public ICommand MoveDownSelectedConfigurationCommand { get; }
        public ICommand MoveUpSelectedConfigurationCommand { get; }

        public ObservableCollection<SettingsPageGenerationItemViewModel> Configurations { get; } = new ObservableCollection<SettingsPageGenerationItemViewModel>();

        [Reactive]
        public SettingsPageGenerationItemViewModel SelectedConfiguration { get; set; }

        [Reactive]
        public IEnumerable<SettingsPageGenerationAvailableGeneratorViewModel> Generators { get; private set; }

        [Reactive]
        public SettingsPageGenerationAvailableGeneratorViewModel SelectedGenerator { get; set; }

        private SettingsPageGenerationItemViewModel ViewModelFromLocalConfiguration(GeneratorItemConfiguration c)
        {
            var g = _generatorCatalogueService.GeneratorInfo[c.GeneratorId];
            var configItem = g.DeserialiseConfiguration(c.ConfigurationStructure);

            return new SettingsPageGenerationItemViewModel(c.Id, g.Id, g.Name, c.Name, c.Description, configItem);
        }

        public override void ApplyConfigChanges()
        {
            var generatorConfigurations = _generatorConfigurations.Get();

            generatorConfigurations.Items = Configurations.Where(c => _generatorCatalogueService.GeneratorInfo.ContainsKey(c.GeneratorId)).Select(c =>
            {
                var g = _generatorCatalogueService.GeneratorInfo[c.GeneratorId];

                return new GeneratorItemConfiguration
                {
                    Id = c.Id,
                    GeneratorId = c.GeneratorId,
                    Name = c.Name,
                    Description = c.Description,
                    ConfigurationStructure = g.SerialiseConfiguration(c.Configuration)
                };
            }).ToArray();

            _generatorConfigurations.Update(generatorConfigurations);
        }

        public override void CancelConfigChanges()
        {
            var generatorConfigurations = _generatorConfigurations.Get();

            Configurations.Clear();

            var configItems = generatorConfigurations.Items.Where(x => _generatorCatalogueService.GeneratorInfo.ContainsKey(x.GeneratorId)).Select(ViewModelFromLocalConfiguration);

            foreach (var c in configItems) Configurations.Add(c);

            SelectedConfiguration = Configurations.FirstOrDefault();

            Generators = _generatorCatalogueService.GeneratorInfo.Values.Select(x => new SettingsPageGenerationAvailableGeneratorViewModel(x)).ToList();
            SelectedGenerator = Generators.FirstOrDefault();
        }
    }
}
