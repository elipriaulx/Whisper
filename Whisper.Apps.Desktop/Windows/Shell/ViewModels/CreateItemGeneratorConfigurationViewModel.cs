using System;
using ReactiveUI;
using Whisper.Core.Models.Generation;
using Whisper.Core.Services;

namespace Whisper.Apps.Desktop.ViewModels
{
    public class CreateItemGeneratorConfigurationViewModel : ReactiveObject
    {
        private readonly IGeneratorService _generator;

        private readonly GeneratorConfigurationBase _configuration;

        public CreateItemGeneratorConfigurationViewModel(Guid id, Guid generatorId, string generatorName, string name, string description, GeneratorConfigurationBase configuration, IGeneratorService generator)
        {
            Id = id;
            GeneratorId = generatorId;
            GeneratorName = generatorName;
            Name = name;
            Description = description;

            _configuration = configuration;

            _generator = generator;
        }

        public Guid Id { get; }

        public Guid GeneratorId { get; }

        public string GeneratorName { get; }

        public string Name { get; }

        public string Description { get; }

        public ContentBase CreateInstance()
        {
            return _generator.Create(GeneratorId, _configuration, Name);
        }
    }
}
