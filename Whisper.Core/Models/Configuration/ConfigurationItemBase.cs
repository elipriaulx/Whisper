using System;
using System.Xml.Serialization;
using UniForm.Core.Attributes.Composition;

namespace Whisper.Core.Models.Configuration
{
    public class ConfigurationItemBase
    {
        [XmlAttribute]
        [UniFormFieldIgnored]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}