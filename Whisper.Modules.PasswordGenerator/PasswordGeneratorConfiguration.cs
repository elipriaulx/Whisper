using System.Xml.Serialization;
using UniForm.Core.Attributes.Composition;
using Whisper.Core.Models.Generation;

namespace Whisper.Modules.PasswordGenerator
{
    public sealed class PasswordGeneratorConfiguration : GeneratorConfigurationBase
    {
        public PasswordGeneratorConfiguration()
        {

        }

        [XmlAttribute]
        [UniFormField("Password Length", "The number of characters expected in the password.", 0)]
        public int PasswordLength { get; set; } = 20;

        [XmlAttribute]
        [UniFormField("Minimum Special Characters", "The minimum number of non-alphanumeric characters expected in the password.", 10)]
        public int MinNonAlphaNumeric { get; set; } = 2;

        [XmlAttribute]
        [UniFormField("Maximum Special Characters", "The maximum number of non-alphanumeric characters expected in the password.", 12)]
        public int MaxNonAlphaNumeric { get; set; } = 8;
    }
}
