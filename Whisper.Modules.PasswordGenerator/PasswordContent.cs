using System.Net;
using Whisper.Core.Models.Generation;

namespace Whisper.Modules.PasswordGenerator
{
    public sealed class PasswordContent : ContentBase
    {
        private readonly string _passwordString;

        public PasswordContent(string passwordString)
        {
            _passwordString = passwordString;

            PublicPreviewText = _passwordString;
            PrivatePreviewText = _passwordString;
            Mdl2Icon = WebUtility.HtmlDecode("&#xE939;");
        }

        public override string PublicPreviewText { get; }

        public override string PrivatePreviewText { get; }

        public override string Mdl2Icon { get; }

        public override void SetToClipboard(IClipboard clipboard)
        {
            clipboard.SetClipboardText(_passwordString);
        }
    }
}
