using System.Xml.Serialization;

namespace Whisper.Core.Models.Generation
{
    public abstract class GeneratorConfigurationBase
    {
        protected GeneratorConfigurationBase(string name)
        {
            Name = name;
        }

        [XmlAttribute]
        public string Name { get; set; }
    }
}
