using System;
using Whisper.Core.Modularity;

namespace Whisper.Modules.PasswordGenerator
{
    public sealed class PasswordGeneratorModule : IWhisperModule
    {
        public Guid Id => Guid.Parse("53CB8B46-EB99-4049-9736-75EE74812327");
        public string Name => nameof(PasswordGeneratorModule);

        public void LoadModule(IComponentRegistry registry)
        {
            registry.RegisterContentGenerator(new PasswordGenerator());
        }
    }
}
