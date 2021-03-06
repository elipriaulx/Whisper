﻿using System;

namespace Whisper.Apps.Common.Models.Configurations
{
    public static class CommonConfigurations
    {
        public static Guid LoggerConfiguration { get; } = Guid.Parse("8A8D4A2C-5ABA-4357-8B81-B6DDDA4E631D");
        public static Guid FactoryMonitorConfiguration { get; } = Guid.Parse("9158FB0E-BD45-4D91-AD69-ED8EEA61CCFD");
        public static Guid GeneratorConfiguration { get; } = Guid.Parse("A2F921C0-D8BA-4A70-B08E-62810595BED0");
    }
}
