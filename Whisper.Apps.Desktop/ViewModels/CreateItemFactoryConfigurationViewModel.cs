using ReactiveUI;
using Whisper.Core.Models.Generation;
using Whisper.Core.Services;

namespace Whisper.Apps.Desktop.ViewModels
{
    public class CreateItemFactoryConfigurationViewModel : ReactiveObject
    {
        private readonly IContentFactoryMeta _contentFactoryMeta;
        private readonly IGeneratorService _generator;

        public string Name => _contentFactoryMeta.Name;
        public string Description => _contentFactoryMeta.Description;

        public CreateItemFactoryConfigurationViewModel(IContentFactoryMeta contentFactoryMeta, IGeneratorService generator)
        {
            _contentFactoryMeta = contentFactoryMeta;
            _generator = generator;
        }

        public ContentBase CreateInstance()
        {
            return _generator.Create(_contentFactoryMeta.Id);
        }
    }
}
