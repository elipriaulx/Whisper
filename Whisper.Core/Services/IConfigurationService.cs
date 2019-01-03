using System;
using System.Xml.Linq;
using Whisper.Core.Models.Configuration;

namespace Whisper.Core.Services
{
    public interface IConfigurationService
    {
        IObservable<ConfigurationChangeInfo> Updated { get; }
        
        IConfigurationAgent GetConfigurationAgent(Guid id, object changeSource = null);
        IConfigurationAgent GetConfigurationAgent(string idString, object changeSource = null);

        IConfigurationAgent<T> GetConfigurationAgent<T>(Guid id, object changeSource = null) where T : ConfigurationItemBase, new();
        IConfigurationAgent<T> GetConfigurationAgent<T>(string idString, object changeSource = null) where T : ConfigurationItemBase, new();

        XElement GetConfigurationElement(Guid id);
        XElement GetConfigurationElement(string idString);

        T GetConfiguration<T>(Guid id) where T : ConfigurationItemBase, new();
        T GetConfiguration<T>(string idString) where T : ConfigurationItemBase, new();

        void SetConfiguration(XElement element, object changeSource = null);
        void SetConfiguration<T>(T obj, object changeSource = null) where T : ConfigurationItemBase;
    }
}