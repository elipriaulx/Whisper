using System;
using System.Reactive.Linq;
using System.Xml.Linq;
using Whisper.Core.Models.Configuration;

namespace Whisper.Apps.Common.Models.Configuration
{
    public class ConfigurationAgent : IConfigurationAgent
    {
        protected CommonConfigurationAgent Agent { get; }
        protected object ChangeSource { get; }

        public ConfigurationAgent(CommonConfigurationAgent agent, object changeSource = null)
        {
            Agent = agent;
            ChangeSource = changeSource;
        }

        public IObservable<ConfigurationChangeInfo> Updated => Agent.Updated.Where(x => ChangeSource == null || ChangeSource == x.ChangeSource);

        public Guid Id => Agent.Id;

        public XElement Element => Agent.Element;

        public void Update(XElement element)
        {
            Agent.Update(element, ChangeSource);
        }

        public T Get<T>() where T : ConfigurationItemBase, new()
        {
            return Agent.Get<T>();
        }

        public void Update<TConvert>(TConvert data) where TConvert : ConfigurationItemBase
        {
            Agent.Update(data, ChangeSource);
        }
    }

    public class ConfigurationAgent<T> : ConfigurationAgent, IConfigurationAgent<T> where T : ConfigurationItemBase, new()
    {
        public ConfigurationAgent(CommonConfigurationAgent agent, object changeSource = null) : base(agent, changeSource)
        {

        }
        
        public void Update(T data)
        {
            Agent.Update(data, ChangeSource);
        }

        public T Get()
        {
            return Agent.Get<T>();
        }
    }
}
