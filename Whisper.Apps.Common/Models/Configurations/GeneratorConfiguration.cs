using System;
using System.Xml.Linq;
using System.Xml.Serialization;
using Whisper.Core.Models.Configuration;

namespace Whisper.Apps.Common.Models.Configurations
{
    public class GeneratorConfiguration : ConfigurationItemBase
    {
        [XmlArray]
        public GeneratorItemConfiguration[] Items { get; set; } = Array.Empty<GeneratorItemConfiguration>();
    }

    public class GeneratorItemConfiguration
    {
        [XmlAttribute]
        public Guid Id { get; set; }

        [XmlAttribute]
        public Guid GeneratorId { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlElement]
        public XElement ConfigurationStructure { get; set; }
    }
}
