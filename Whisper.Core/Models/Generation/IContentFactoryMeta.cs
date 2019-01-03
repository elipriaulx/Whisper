using System;

namespace Whisper.Core.Models.Generation
{
    public interface IContentFactoryMeta
    {
        Guid Id { get; }

        string Name { get; }

        string Description { get; }
    }
}
