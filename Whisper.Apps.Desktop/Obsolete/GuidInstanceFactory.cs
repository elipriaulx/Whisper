using System;
using Whisper.Core.Models.Generation;

namespace Whisper.Apps.Desktop.Obsolete
{
    public class GuidInstanceFactory : IContentFactory
    {
        public Guid Id { get; } = Guid.Parse("40288B09-A80F-4FA2-86EC-BC3AACCDA257");

        public string Name { get; } = "Guid Factory";

        public string Description { get; } = "Generates Guids.";

        public ContentBase CreateDefaultContentInstance()
        {
            var instance = new GuidContent();

            return instance;
        }

        public class GuidContent : ContentBase
        {
            public GuidContent() : base(Guid.Parse("40288B09-A80F-4FA2-86EC-BC3AACCDA257"), "Guid")
            {
                SetPreviewText(ContentId.ToString());
                SetMdl2Icon(System.Net.WebUtility.HtmlDecode("&#xE943;"));
            }

            public override void SetToClipboard(IClipboard clipboard)
            {
                clipboard.SetClipboardText(ContentId.ToString());
            }
        }
    }
}
