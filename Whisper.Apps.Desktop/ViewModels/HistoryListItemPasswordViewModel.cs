using System;
using System.Security.Cryptography;
using System.Web.Security;

namespace Whisper.Apps.Desktop.ViewModels
{
    public class HistoryListItemPasswordViewModel : HistoryListItemViewModel
    {
        private const int PasswordLength = 16;
        private const int MinNonAlphaNumeric = 2;
        private const int MaxNonAlphaNumeric = 8;
        private const int RangeNonAlphaNumeric = MaxNonAlphaNumeric - MinNonAlphaNumeric;

        private readonly string _passwordContent;

        public HistoryListItemPasswordViewModel()
        {
            using (var rnd = RandomNumberGenerator.Create())
            {
                var byteArray = new byte[4];
                rnd.GetBytes(byteArray);
                var randomInt = Math.Abs(BitConverter.ToInt32(byteArray, 0) % RangeNonAlphaNumeric);

                _passwordContent = Membership.GeneratePassword(PasswordLength, MinNonAlphaNumeric + randomInt);
            }
        }

        public override string ContentDescription => "Password";

        public override string ContentPreview => _passwordContent;

        public override string Icon => System.Net.WebUtility.HtmlDecode("&#xE939;");

        protected override string GetClipboardContents()
        {
            return _passwordContent;
        }
    }
}
