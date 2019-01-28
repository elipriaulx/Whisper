using System;

namespace Whisper.Core.Models.Generation
{
    public abstract class ContentBase
    {
        protected ContentBase()
        {
            ContentId = Guid.NewGuid();
        }

        internal void SetSource(Guid generatorId, string generatorName)
        {
            GeneratorId = generatorId;
            Name = generatorName;
        }
        

        internal void SetName(string contentName)
        {
            Name = contentName;
        }
        
        public Guid ContentId { get; }

        public Guid GeneratorId { get; private set; }

        public string Name { get; private set; } = "Unnamed";

        public abstract string PublicPreviewText { get; }

        public abstract string PrivatePreviewText { get; }

        public abstract string Mdl2Icon { get; }
        
        public abstract void SetToClipboard(IClipboard clipboard);
    }
}
