using System;
using System.IO;
using System.Reactive.Subjects;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Whisper.Core.Models.Configuration;
using Whisper.Core.Models.Logging;

namespace Whisper.Apps.Common.Models.Configuration
{
    public class CommonConfigurationAgent
    {
        private readonly ILogger _logger;
        private readonly Subject<ConfigurationChangeInfo> _updated = new Subject<ConfigurationChangeInfo>();

        public CommonConfigurationAgent(Guid id, ILogger logger = null)
        {
            _logger = logger;

            Id = id;
            Update(new ConfigurationItemBase
            {
                Id = id
            });
        }
        
        public IObservable<ConfigurationChangeInfo> Updated => _updated;

        public Guid Id { get; }

        public XElement Element { get; private set; }

        public void Update(XElement element, object changeSource = null)
        {
            Element = element;

            RaiseUpdate(changeSource);
        }

        public T Get<T>() where T : ConfigurationItemBase, new()
        {
            T result;

            try
            {
                var reader = Element.CreateReader();
                var xRoot = new XmlRootAttribute
                {
                    ElementName = ConfigurationConstants.ConfigurationEntityTag,
                    IsNullable = true
                };

                var serializer = new XmlSerializer(typeof(T), xRoot);

                result = (T) serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                _logger?.Warning($"Deserialisation of local configuration item '{Id}' with type '{typeof(T).Name}' failed.");
                _logger?.Warning(ex);

                result = new T
                {
                    Id = Id
                };
            }

            return result;
        }

        public void Update<T>(T obj, object changeSource = null) where T : ConfigurationItemBase
        {
            if (obj.Id != Id)
                throw new ArgumentException("The provided configuration object must have a matching Id.");

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var streamWriter = new StreamWriter(memoryStream))
                    {
                        var xRoot = new XmlRootAttribute
                        {
                            ElementName = ConfigurationConstants.ConfigurationEntityTag,
                            IsNullable = true
                        };

                        var xmlSerializer = new XmlSerializer(typeof(T), xRoot);

                        var ns = new XmlSerializerNamespaces();
                        ns.Add("", "");

                        xmlSerializer.Serialize(streamWriter, obj, ns);

                        Element = XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));

                        RaiseUpdate(changeSource);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.Warning($"Serialisation of local configuration item '{Id}' with type '{typeof(T).Name}' failed.");
                _logger?.Warning(ex);

                throw new ArgumentException("Unable to serialise the provided object.", ex);
            }
        }

        private void RaiseUpdate(object changeSource)
        {
            try
            {
                _updated.OnNext(new ConfigurationChangeInfo(Id, changeSource));
            }
            catch (Exception ex)
            {
                _logger?.Error($"Unable to raise notifications when updating local configuration item '{Id}'.");
                _logger?.Error(ex);
            }
        }
    }
}