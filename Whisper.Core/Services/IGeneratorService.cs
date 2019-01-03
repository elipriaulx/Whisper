using System;
using System.Collections.Generic;
using System.Reactive;
using Whisper.Core.Models.Generation;

namespace Whisper.Core.Services
{
    public interface IGeneratorService
    {
        IReadOnlyDictionary<Guid, IContentFactory> FactoryInfo { get; } // TODO: Fix

        IReadOnlyList<ContentBase> Content { get; }

        IObservable<IContentFactoryMeta> FactoryAdded { get; }

        IObservable<Unit> ContentCleared { get; }

        IObservable<ContentBase> ContentCreated { get; }

        
        void AddFactory(IContentFactory factory);

        ContentBase Create(Guid factoryId);

        void ClearHistory();
    }
}