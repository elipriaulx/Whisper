using System;
using Whisper.Core.Modularity;

namespace Whisper.Modules.NumberGenerator
{
    public sealed class NumberGeneratorModule : IWhisperModule
    {
        public Guid Id => Guid.Parse("DF6B5950-0F30-4701-8A31-9E1A9B5FF6E0");
        public string Name => nameof(NumberGeneratorModule);

        public void LoadModule(IComponentRegistry registry)
        {
            registry.RegisterContentGenerator(new NumberGenerator());
        }
    }
}
