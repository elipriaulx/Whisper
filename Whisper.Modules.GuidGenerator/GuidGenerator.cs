using System;
using Whisper.Core.Models.Generation;

namespace Whisper.Modules.GuidGenerator
{
    public sealed class GuidGenerator : ContentGeneratorBase<GuidContent, GuidGeneratorConfiguration>
    {
        public override Guid Id => Guid.Parse("E9AAC6A8-EBC3-43C8-A241-E1423A0ABB83");
        public override string Name => "Guid";
        public override string Description => "Generates UUIDs.";

        protected override GuidContent Create(GuidGeneratorConfiguration configuration)
        {
            var guid = Guid.NewGuid();

            string formatString;

            switch (configuration.Format)
            {
                case GuidFormats.MsGuidN:
                    formatString = "N";
                    break;
                default:
                case GuidFormats.MsGuidD:
                    formatString = "D";
                    break;
                case GuidFormats.MsGuidB:
                    formatString = "B";
                    break;
                case GuidFormats.MsGuidP:
                    formatString = "P";
                    break;
                case GuidFormats.MsGuidX:
                    formatString = "X";
                    break;
            }

            var guidString = guid.ToString(formatString);
            guidString = configuration.Casing == GuidCasing.Lowercase ? guidString.ToLower() : guidString.ToUpper();

            var content = $"{configuration.Prefix}{guidString}{configuration.Suffix}";

            return new GuidContent(content);
        }
    }
}
