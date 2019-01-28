using UniForm.Core.Attributes.Composition;
using Whisper.Core.Models.Generation;

namespace Whisper.Modules.GuidGenerator
{
    [UniForm]
    public sealed class GuidGeneratorConfiguration : GeneratorConfigurationBase
    {
        [UniFormField("Guid Format", priority: 0)]
        public GuidFormats Format { get; set; } = GuidFormats.MsGuidD;

        [UniFormField("Guid Casing", priority: 20)]
        public GuidCasing Casing { get; set; } = GuidCasing.Uppercase;

        [UniFormField("Prefix", priority: 100)]
        public string Prefix { get; set; } = string.Empty;

        [UniFormField("Suffix", priority: 110)]
        public string Suffix { get; set; } = string.Empty;
    }
}
