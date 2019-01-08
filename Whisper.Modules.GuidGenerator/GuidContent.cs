using Whisper.Core.Models.Generation;

namespace Whisper.Modules.GuidGenerator
{
    public sealed class GuidContent : ContentBase
    {
        private readonly string _guidString;

        public GuidContent(string guidString)
        {
            _guidString = guidString;

            PublicPreviewText = _guidString;
            PrivatePreviewText = _guidString;
            Mdl2Icon = System.Net.WebUtility.HtmlDecode("&#xE943;");
        }

        public override string PublicPreviewText { get; }

        public override string PrivatePreviewText { get; }

        public override string Mdl2Icon { get; }

        public override void SetToClipboard(IClipboard clipboard)
        {
            clipboard.SetClipboardText(_guidString);
        }
    }
}
