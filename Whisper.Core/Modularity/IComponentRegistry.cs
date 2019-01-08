using Whisper.Core.Models.Generation;

namespace Whisper.Core.Modularity
{
    public interface IComponentRegistry
    {
        void RegisterContentGenerator(ContentGeneratorBase contentGenerator);
    }
}