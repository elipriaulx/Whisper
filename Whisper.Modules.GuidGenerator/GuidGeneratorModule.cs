using System;
using Whisper.Extensibility.Modularity;

namespace Whisper.Modules.GuidGenerator
{
    public class GuidGeneratorModule : IWhisperModule
    {
        public Guid Id => Guid.Parse("74080dc9-ef0f-4792-8323-524a89c56fc5");
        public string Name => nameof(GuidGeneratorModule);

        public void LoadModule(IGeneratorRegistry registry)
        {

        }
    }
}