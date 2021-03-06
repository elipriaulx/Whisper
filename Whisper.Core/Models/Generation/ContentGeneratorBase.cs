﻿using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Whisper.Core.Models.Generation
{
    public abstract class ContentGeneratorBase : IContentGenerator
    {
        public abstract Guid Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }

        public abstract ContentBase CreateInstance(string name = null);
        public abstract ContentBase CreateInstance(XElement configuration, string name = null);
        public abstract ContentBase CreateInstance(GeneratorConfigurationBase generatorConfiguration, string name = null);

        public abstract GeneratorConfigurationBase CreateConfiguration();

        public abstract XElement SerialiseConfiguration(GeneratorConfigurationBase generatorConfiguration);
        public abstract GeneratorConfigurationBase DeserialiseConfiguration(XElement generatorConfiguration);
    }

    public abstract class ContentGeneratorBase<TContent, TConfiguration> : ContentGeneratorBase where TContent : ContentBase where TConfiguration : GeneratorConfigurationBase, new()
    {
        public override ContentBase CreateInstance(string name = null)
        {
            return TaggedCreate(name: name);
        }

        public override ContentBase CreateInstance(XElement configuration, string name = null)
        {
            var configInstance = DoDeserialiseConfiguration(configuration);

            return TaggedCreate(configInstance, name);
        }

        public override ContentBase CreateInstance(GeneratorConfigurationBase generatorConfiguration, string name = null)
        {
            var configInstance = ToTypedConfiguration(generatorConfiguration);

            return TaggedCreate(configInstance, name);
        }

        public override GeneratorConfigurationBase CreateConfiguration()
        {
            return DoCreateConfiguration();
        }

        private TConfiguration ToTypedConfiguration(GeneratorConfigurationBase generatorConfiguration)
        {
            var configInstance = generatorConfiguration as TConfiguration;

            if (configInstance == null)
                throw new ArgumentException($"'generatorConfiguration' is not of type '{typeof(TConfiguration).Name}'.");

            return configInstance;
        }

        public override XElement SerialiseConfiguration(GeneratorConfigurationBase generatorConfiguration)
        {
            var configInstance = ToTypedConfiguration(generatorConfiguration);

            return DoSerialiseConfiguration(configInstance);
        }

        public override GeneratorConfigurationBase DeserialiseConfiguration(XElement generatorConfiguration)
        {
            return DoDeserialiseConfiguration(generatorConfiguration);
        }

        private TContent TaggedCreate(TConfiguration configuration = null, string name = null)
        {
            if (configuration == null)
                configuration = DoCreateConfiguration();

            var content = Create(configuration);

            content.SetSource(Id, Name);

            if (name != null)
                content.SetName(name);

            return content;
        }

        protected abstract TContent Create(TConfiguration configuration);

        protected virtual TConfiguration DoCreateConfiguration()
        {
            return new TConfiguration();
        }

        protected virtual XElement DoSerialiseConfiguration(TConfiguration generatorConfiguration)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var streamWriter = new StreamWriter(memoryStream))
                    {
                        var xRoot = new XmlRootAttribute
                        {
                            ElementName = EntityTag,
                            IsNullable = true
                        };

                        var xmlSerializer = new XmlSerializer(typeof(TConfiguration), xRoot);

                        var ns = new XmlSerializerNamespaces();
                        ns.Add("", "");

                        xmlSerializer.Serialize(streamWriter, generatorConfiguration, ns);

                        return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: wrap exceptions
                throw;
            }
        }

        private const string EntityTag = "ContentConfig";

        protected virtual TConfiguration DoDeserialiseConfiguration(XElement generatorConfiguration)
        {
            TConfiguration result;

            try
            {
                var reader = generatorConfiguration.CreateReader();
                var xRoot = new XmlRootAttribute
                {
                    ElementName = EntityTag,
                    IsNullable = true
                };

                var serializer = new XmlSerializer(typeof(TConfiguration), xRoot);

                result = (TConfiguration)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                // TODO: Wrap errors
                throw;
            }

            return result;
        }
    }
}
