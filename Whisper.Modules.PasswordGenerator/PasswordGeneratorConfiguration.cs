using System.Xml.Serialization;
using Whisper.Core.Models.Generation;

namespace Whisper.Modules.PasswordGenerator
{
    public sealed class PasswordGeneratorConfiguration : GeneratorConfigurationBase
    {
        public PasswordGeneratorConfiguration() : base("Password")
        {

        }

        [XmlAttribute]
        public int PasswordLength { get; set; } = 20;

        [XmlAttribute]
        public int MinNonAlphaNumeric { get; set; } = 2;

        [XmlAttribute]
        public int MaxNonAlphaNumeric { get; set; } = 8;
    }
}
