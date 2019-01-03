using System;

namespace Whisper.Core.Models.Generation
{
    public interface IContentFactory : IContentFactoryMeta
    {
        ContentBase CreateDefaultContentInstance();


    }
}
