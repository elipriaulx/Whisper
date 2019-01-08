using System.Net;
using Whisper.Core.Models.Generation;

namespace Whisper.Modules.NumberGenerator
{
    public sealed class NumberContent : ContentBase
    {
        private readonly string _numberString;

        public NumberContent(string numberString)
        {
            _numberString = numberString;

            PublicPreviewText = _numberString;
            PrivatePreviewText = _numberString;
            Mdl2Icon = WebUtility.HtmlDecode("&#xF146;");
        }

        public override string PublicPreviewText { get; }

        public override string PrivatePreviewText { get; }

        public override string Mdl2Icon { get; }

        public override void SetToClipboard(IClipboard clipboard)
        {
            clipboard.SetClipboardText(_numberString);
        }
    }
}
