using System;

namespace Whisper.Core.Models.Configuration
{
    public class ConfigurationChangeInfo
    {
        public ConfigurationChangeInfo(Guid configurationId, object changeSource = null)
        {
            ConfigurationId = configurationId;
            ChangeSource = changeSource;
        }

        public Guid ConfigurationId { get; }
        public object ChangeSource { get; }
    }
}
