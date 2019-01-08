using System;
using Whisper.Core.Modularity;

namespace Whisper.Modules.GuidGenerator
{
    public sealed class GuidGeneratorModule : IWhisperModule
    {
        public Guid Id => Guid.Parse("4E537619-9A16-43CF-91D6-2365CC38B8B1");
        public string Name => nameof(GuidGeneratorModule);

        public void LoadModule(IComponentRegistry registry)
        {
            registry.RegisterContentGenerator(new GuidGenerator());
        }
    }
}