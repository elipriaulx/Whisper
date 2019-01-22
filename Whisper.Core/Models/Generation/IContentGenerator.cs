using System;
using System.Xml.Linq;

namespace Whisper.Core.Models.Generation
{
    public interface IContentGenerator : IContentGeneratorMeta
    {
        ContentBase CreateInstance();
        ContentBase CreateInstance(XElement generatorConfiguration);
        ContentBase CreateInstance(GeneratorConfigurationBase generatorConfiguration);

        GeneratorConfigurationBase CreateConfiguration();

        XElement SerialiseConfiguration(GeneratorConfigurationBase generatorConfiguration);
        GeneratorConfigurationBase DeserialiseConfiguration(XElement generatorConfiguration);
    }
}
