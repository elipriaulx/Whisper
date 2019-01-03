using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Xml.Linq;
using Whisper.Apps.Common.Models.Configuration;
using Whisper.Core.Models.Configuration;
using Whisper.Core.Models.Logging;
using Whisper.Core.Services;

namespace Whisper.Apps.Common.Services
{
    public class ConfigurationServiceProvider : IConfigurationService
    {
        private readonly CompositeDisposable _agentUpdateSubscriptions = new CompositeDisposable();
        private readonly Dictionary<Guid, CommonConfigurationAgent> _configDictionary = new Dictionary<Guid, CommonConfigurationAgent>();

        private readonly ILogger _logger;

        private readonly Subject<ConfigurationChangeInfo> _updated = new Subject<ConfigurationChangeInfo>();

        public ConfigurationServiceProvider(ILoggingService loggingService)
        {
            _logger = loggingService.GetDefaultLogger();
        }
        
        public IObservable<ConfigurationChangeInfo> Updated => _updated;

        public IConfigurationAgent GetConfigurationAgent(Guid id, object changeSource = null)
        {
            var agent = GetConfigurationAgentInternal(id, changeSource);

            return new ConfigurationAgent(agent, changeSource);
        }

        public IConfigurationAgent GetConfigurationAgent(string idString, object changeSource = null)
        {
            var agent = GetConfigurationAgentInternal(idString);

            return new ConfigurationAgent(agent, changeSource);
        }

        public IConfigurationAgent<T> GetConfigurationAgent<T>(Guid id, object changeSource = null) where T : ConfigurationItemBase, new()
        {
            var agent = GetConfigurationAgentInternal(id);

            return new ConfigurationAgent<T>(agent, changeSource);
        }

        public IConfigurationAgent<T> GetConfigurationAgent<T>(string idString, object changeSource = null) where T : ConfigurationItemBase, new()
        {
            var agent = GetConfigurationAgentInternal(idString);

            return new ConfigurationAgent<T>(agent, changeSource);
        }

        public XElement GetConfigurationElement(Guid id)
        {
            var agent = GetConfigurationAgent(id);

            return agent.Element;
        }

        public XElement GetConfigurationElement(string idString)
        {
            Guid id;

            try
            {
                id = Guid.Parse(idString);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(nameof(idString), ex);
            }

            return GetConfigurationElement(id);
        }

        public T GetConfiguration<T>(string idString) where T : ConfigurationItemBase, new()
        {
            Guid id;

            try
            {
                id = Guid.Parse(idString);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(nameof(idString), ex);
            }

            return GetConfiguration<T>(id);
        }

        public T GetConfiguration<T>(Guid id) where T : ConfigurationItemBase, new()
        {
            var agent = GetConfigurationAgent(id);

            return agent.Get<T>();
        }

        public void SetConfiguration(XElement element, object changeSource = null)
        {
            Guid id;

            try
            {
                id = (Guid) element.Attribute("Id");
            }
            catch (Exception ex)
            {
                _logger?.Warning("Attempt was made to persist configuration without Id.");
                throw new ArgumentException("Unable to persist XElement without Id.", ex);
            }

            var agent = GetConfigurationAgent(id);

            agent.Update(element);
        }

        public void SetConfiguration<T>(T obj, object changeSource = null) where T : ConfigurationItemBase
        {
            var agent = GetConfigurationAgent(obj.Id);

            agent.Update(obj);
        }
        
        public void LoadConfiguration(string fileName)
        {
            _logger?.Info($"Loading configuration from file '{fileName}'.");

            _logger?.Debug("Clearing configuration dictionary.");
            _configDictionary.Clear();

            CommonConfigurationAgent GetOrCreate(Guid id)
            {
                if (_configDictionary.ContainsKey(id))
                    return _configDictionary[id];

                var agent = new CommonConfigurationAgent(id, _logger);

                var s = agent.Updated.Do(Agent_Updated).Subscribe();
                _agentUpdateSubscriptions.Add(s);

                _configDictionary.Add(id, agent);

                return agent;
            }


            if (File.Exists(fileName))
                try
                {
                    _logger?.Debug("Loading configuration xml document.");
                    var doc = XDocument.Load(fileName);

                    var xmlElements = doc.Root?.Elements(ConfigurationConstants.ConfigurationEntityTag);

                    if (xmlElements == null)
                        return;

                    foreach (var el in xmlElements)
                    {
                        var id = (Guid) el.Attribute("Id");

                        _logger?.Debug($"Entity discovered with Id '{id}'.");

                        var agent = GetOrCreate(id);

                        agent.Update(el, this);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.Error("An error occured whilst loading configuration.");
                    _logger?.Error(ex);

                    _configDictionary.Clear();
                    throw new Exception("Unable to load configuration from the specified path.", ex);
                }
            else
                _logger?.Warning("An attempt was made to load configuration that doesn't exist.");
        }

        public void SaveConfiguration(string fileName)
        {
            _logger?.Info($"Loading configuration to file '{fileName}'.");

            var doc = new XDocument(new XElement(ConfigurationConstants.RootEntityTag));

            // Create Directory
            try
            {
                var dir = Path.GetDirectoryName(fileName);
                // ReSharper disable once AssignNullToNotNullAttribute
                Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                _logger?.Error("Unable to create configuration output location.");
                _logger?.Error(ex);

                throw;
            }

            try
            {
                foreach (var cfg in _configDictionary.Values)
                {
                    var element = cfg.Element;

                    if (element != null) doc.Root.Add(element);
                }

                doc.Save(fileName);
            }
            catch (Exception ex)
            {
                _logger?.Error("An error occured saving the configuration file.");
                _logger?.Error(ex);
                throw;
            }
        }
        
        private CommonConfigurationAgent GetConfigurationAgentInternal(Guid id, object changeSource = null)
        {
            if (_configDictionary.ContainsKey(id))
                return _configDictionary[id];

            // Create a new configuration entity
            var agent = new CommonConfigurationAgent(id, _logger);

            var s = agent.Updated.Do(Agent_Updated).Subscribe();
            _agentUpdateSubscriptions.Add(s);

            _configDictionary.Add(id, agent);

            return agent;
        }

        private CommonConfigurationAgent GetConfigurationAgentInternal(string idString)
        {
            Guid id;

            try
            {
                id = Guid.Parse(idString);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(nameof(idString), ex);
            }

            return GetConfigurationAgentInternal(id);
        }
        
        private void Agent_Updated(ConfigurationChangeInfo info)
        {
            // A configuration agent has updated!
            _updated.OnNext(info);
        }
    }
}
