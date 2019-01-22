using System;
using ReactiveUI;
using Whisper.Apps.Common.Models.Configurations;
using Whisper.Core.Models.Generation;

namespace Whisper.Apps.Desktop.Windows.Settings.ViewModels
{
    public class SettingsPageGenerationAvailableGeneratorViewModel : ReactiveObject
    {
        private readonly IContentGenerator _generator;

        public SettingsPageGenerationAvailableGeneratorViewModel(IContentGenerator generator)
        {
            _generator = generator;
            Id = generator.Id;
            Name = generator.Name;
            Description = generator.Description;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public GeneratorItemConfiguration CreateConfigurationInstance()
        {
            var configItem = _generator.CreateConfiguration();
            var configXml = _generator.SerialiseConfiguration(configItem);

            return new GeneratorItemConfiguration
            {
                Id = Guid.NewGuid(),
                GeneratorId = _generator.Id,
                Name = _generator.Name,
                Description = $"New {_generator.Name} Configuration",
                ConfigurationStructure = configXml
            };
        }
    }
}
