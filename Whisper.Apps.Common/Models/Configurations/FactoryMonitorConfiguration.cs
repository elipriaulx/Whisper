using System.Xml.Serialization;
using Whisper.Core.Models.Configuration;

namespace Whisper.Apps.Common.Models.Configurations
{
    public class FactoryMonitorConfiguration : ConfigurationItemBase
    {
        [XmlAttribute]
        public bool EnableAutoCopy { get; set; } = true;
    }
}
