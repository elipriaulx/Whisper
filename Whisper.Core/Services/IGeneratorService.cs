using System;
using System.Collections.Generic;
using System.Reactive;
using Whisper.Core.Models.Generation;

namespace Whisper.Core.Services
{
    public interface IGeneratorService : IGeneratorCatalogueService
    {

        IReadOnlyList<ContentBase> Content { get; }

        IObservable<Unit> ContentCleared { get; }

        IObservable<ContentBase> ContentCreated { get; }
        
        ContentBase Create(Guid factoryId, string name = null);

        ContentBase Create(Guid factoryId, GeneratorConfigurationBase configuration, string name = null);

        void ClearHistory();
    }
}