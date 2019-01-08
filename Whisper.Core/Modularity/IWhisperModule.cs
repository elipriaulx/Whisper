using System;

namespace Whisper.Core.Modularity
{
    public interface IWhisperModule
    {
        Guid Id { get; }

        string Name { get; }

        void LoadModule(IComponentRegistry registry);
    }
}