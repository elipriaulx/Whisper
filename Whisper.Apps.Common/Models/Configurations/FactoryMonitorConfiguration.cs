using System.Xml.Serialization;
using UniForm.Core.Attributes.Composition;
using Whisper.Core.Models.Configuration;

namespace Whisper.Apps.Common.Models.Configurations
{
    [UniForm]
    public class FactoryMonitorConfiguration : ConfigurationItemBase
    {
        [XmlAttribute]
        [UniFormField("Auto-Copy", "Enables automatic copying of generated content to the system clipboard.")]
        public bool EnableAutoCopy { get; set; } = true;
    }
}
