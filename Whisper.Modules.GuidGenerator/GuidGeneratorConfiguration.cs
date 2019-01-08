using Whisper.Core.Models.Generation;

namespace Whisper.Modules.GuidGenerator
{
    public sealed class GuidGeneratorConfiguration : GeneratorConfigurationBase
    {
        public GuidGeneratorConfiguration() : base("Guid")
        {
        }
    }
}
