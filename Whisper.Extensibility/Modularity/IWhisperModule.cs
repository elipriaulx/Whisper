using System;

namespace Whisper.Extensibility.Modularity
{
    public interface IWhisperModule
    {
        Guid Id { get; }

        string Name { get; }

        void LoadModule(IGeneratorRegistry registry);
    }
}