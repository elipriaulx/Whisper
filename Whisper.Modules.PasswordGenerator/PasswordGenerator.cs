using System;
using System.Security.Cryptography;
using System.Web.Security;
using Whisper.Core.Models.Generation;

namespace Whisper.Modules.PasswordGenerator
{
    public sealed class PasswordGenerator : ContentGeneratorBase<PasswordContent, PasswordGeneratorConfiguration>
    {
        public override Guid Id => Guid.Parse("7A87B7A1-BB91-4E69-8B51-EE7608A55E89");
        public override string Name => "Password";
        public override string Description => "Generates random passwords.";

        protected override PasswordContent Create(PasswordGeneratorConfiguration configuration)
        {
            var range = configuration.MaxNonAlphaNumeric - configuration.MinNonAlphaNumeric;
            string password;

            using (var rnd = RandomNumberGenerator.Create())
            {
                var byteArray = new byte[4];
                rnd.GetBytes(byteArray);
                var randomInt = Math.Abs(BitConverter.ToInt32(byteArray, 0) % range);

                password = Membership.GeneratePassword(configuration.PasswordLength, configuration.MinNonAlphaNumeric + randomInt);
            }

            return new PasswordContent(password);
        }
    }

}
