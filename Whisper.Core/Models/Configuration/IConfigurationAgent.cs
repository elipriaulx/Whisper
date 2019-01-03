using System;
using System.Xml.Linq;

namespace Whisper.Core.Models.Configuration
{
    public interface IConfigurationAgent
    {
        IObservable<ConfigurationChangeInfo> Updated { get; }

        Guid Id { get; }

        XElement Element { get; }

        void Update(XElement element);

        T Get<T>() where T : ConfigurationItemBase, new();

        void Update<T>(T obj) where T : ConfigurationItemBase;
    }

    public interface IConfigurationAgent<T> : IConfigurationAgent where T : ConfigurationItemBase, new()
    {
        void Update(T data);

        T Get();
    }
}
