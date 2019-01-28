using System;
using System.Xml.Linq;

namespace Whisper.Core.Models.Generation
{
    public interface IContentGenerator : IContentGeneratorMeta
    {
        ContentBase CreateInstance(string name = null);
        ContentBase CreateInstance(XElement generatorConfiguration, string name = null);
        ContentBase CreateInstance(GeneratorConfigurationBase generatorConfiguration, string name = null);

        GeneratorConfigurationBase CreateConfiguration();

        XElement SerialiseConfiguration(GeneratorConfigurationBase generatorConfiguration);
        GeneratorConfigurationBase DeserialiseConfiguration(XElement generatorConfiguration);
    }
}
