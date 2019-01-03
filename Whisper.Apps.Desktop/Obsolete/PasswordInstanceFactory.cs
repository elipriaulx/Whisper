using System;
using System.Security.Cryptography;
using System.Web.Security;
using Whisper.Core.Models.Generation;

namespace Whisper.Apps.Desktop.Obsolete
{
    public class PasswordInstanceFactory : IContentFactory
    {
        public Guid Id { get; } = Guid.Parse("B51BBFC5-5D54-4451-9321-DB6B46C9523F");

        public string Name { get; } = "Password Factory";

        public string Description { get; } = "Generates Passwords.";

        public ContentBase CreateDefaultContentInstance()
        {
            var instance = new PasswordContent();

            return instance;
        }

        public class PasswordContent : ContentBase
        {
            private const int PasswordLength = 20;
            private const int MinNonAlphaNumeric = 2;
            private const int MaxNonAlphaNumeric = 8;
            private const int RangeNonAlphaNumeric = MaxNonAlphaNumeric - MinNonAlphaNumeric;

            private readonly string _passwordContent;
            
            public PasswordContent() : base(Guid.Parse("B51BBFC5-5D54-4451-9321-DB6B46C9523F"), "Password")
            {
                using (var rnd = RandomNumberGenerator.Create())
                {
                    var byteArray = new byte[4];
                    rnd.GetBytes(byteArray);
                    var randomInt = Math.Abs(BitConverter.ToInt32(byteArray, 0) % RangeNonAlphaNumeric);

                    _passwordContent = Membership.GeneratePassword(PasswordLength, MinNonAlphaNumeric + randomInt);
                }

                SetPreviewText(_passwordContent);
                SetMdl2Icon(System.Net.WebUtility.HtmlDecode("&#xE939;"));
            }

            public override void SetToClipboard(IClipboard clipboard)
            {
                clipboard.SetClipboardText(_passwordContent);
            }
        }
    }
}
