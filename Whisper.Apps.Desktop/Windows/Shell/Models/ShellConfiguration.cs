using System.Xml.Serialization;
using Whisper.Core.Models.Configuration;

namespace Whisper.Apps.Desktop.Windows.Shell.Models
{
    public class ShellConfiguration : ConfigurationItemBase
    {
        [XmlAttribute]
        public bool AlwaysOnTop { get; set; } = true;
    }
}
