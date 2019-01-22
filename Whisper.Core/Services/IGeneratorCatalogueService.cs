using System;
using System.Collections.Generic;
using Whisper.Core.Models.Generation;

namespace Whisper.Core.Services
{
    public interface IGeneratorCatalogueService
    {
        IReadOnlyDictionary<Guid, IContentGenerator> GeneratorInfo { get; } // TODO: Fix

        IObservable<IContentGeneratorMeta> GeneratorAdded { get; }

        void AddFactory(IContentGenerator generator);
    }
}