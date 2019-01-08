using System;

namespace Whisper.Core.Models.Generation
{
    public abstract class ContentBase
    {
        protected ContentBase()
        {
            ContentId = Guid.NewGuid();
        }

        internal void SetMeta(Guid factoryId, string contentName)
        {
            FactoryId = factoryId;
            Name = contentName;
        }
        
        public Guid ContentId { get; }

        public Guid FactoryId { get; private set; }

        public string Name { get; private set; }

        public abstract string PublicPreviewText { get; }

        public abstract string PrivatePreviewText { get; }

        public abstract string Mdl2Icon { get; }
        
        public abstract void SetToClipboard(IClipboard clipboard);
    }
}
