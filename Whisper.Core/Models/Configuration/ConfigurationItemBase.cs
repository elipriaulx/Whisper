using System;
using System.Xml.Serialization;

namespace Whisper.Core.Models.Configuration
{
    public class ConfigurationItemBase
    {
        [XmlAttribute]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}