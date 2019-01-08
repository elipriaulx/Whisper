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

            return new GuidContent(guid.ToString());
        }
    }
}
