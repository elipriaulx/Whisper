using System;
using System.Reactive;
using System.Reactive.Subjects;

namespace Whisper.Core.Models.Generation
{
    public abstract class ContentBase
    {
        private readonly Subject<Unit> _updated = new Subject<Unit>();

        protected ContentBase(Guid factoryId, string contentName)
        {
            FactoryId = factoryId;
            ContentId = Guid.NewGuid();
            Name = contentName;
        }

        public IObservable<Unit> Updated => _updated;

        public Guid FactoryId { get; }

        public Guid ContentId { get; }

        public string Name { get; }

        public string PreviewText { get; private set; }

        public string Mdl2Icon { get; private set; }

        protected void SetMdl2Icon(string icon)
        {
            Mdl2Icon = icon;
            _updated.OnNext(new Unit());
        }

        protected void SetPreviewText(string preview)
        {
            PreviewText = preview;
            _updated.OnNext(new Unit());
        }

        public abstract void SetToClipboard(IClipboard clipboard);
    }
}
