using System;

namespace Whisper.Core.Models.Generation
{
    public interface IContentGeneratorMeta
    {
        Guid Id { get; }

        string Name { get; }

        string Description { get; }
    }
}
