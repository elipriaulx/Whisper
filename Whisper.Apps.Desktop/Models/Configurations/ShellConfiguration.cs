using System.Xml.Serialization;
using Whisper.Core.Models.Configuration;

namespace Whisper.Apps.Desktop.Models.Configurations
{
    public class ShellConfiguration : ConfigurationItemBase
    {
        [XmlAttribute]
        public bool EnableAlwaysOnTop { get; set; } = true;

        [XmlAttribute]
        public bool EnableMinimiseToTray { get; set; } = true;
    }
}
