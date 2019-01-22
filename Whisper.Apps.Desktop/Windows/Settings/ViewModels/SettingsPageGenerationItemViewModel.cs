using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using UniForm.Core.Attributes.Composition;
using Whisper.Core.Models.Generation;

namespace Whisper.Apps.Desktop.Windows.Settings.ViewModels
{
    [UniForm(automaticPropertyInclusion: false)]
    public class SettingsPageGenerationItemViewModel : ReactiveObject
    {
        public SettingsPageGenerationItemViewModel(Guid id, Guid generatorId, string generatorName, string name, string description, GeneratorConfigurationBase configuration)
        {
            Id = id;
            GeneratorId = generatorId;
            GeneratorName = generatorName;
            Name = name;
            Description = description;
            Configuration = configuration;
        }
        
        public Guid Id { get; }
        
        public Guid GeneratorId { get; }
        
        public string GeneratorName { get; }

        [Reactive]
        [UniFormField(priority: 0)]
        public string Name { get; set; }

        [Reactive]
        [UniFormField(priority: 100)]
        public string Description { get; set; }

        [Reactive]
        public GeneratorConfigurationBase Configuration { get; set; }
    }
}