using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using Whisper.Core.Models.Generation;
using Whisper.Core.Services;

namespace Whisper.Apps.Common.Services
{
    public class GeneratorServiceProvider : IGeneratorService
    {
        private readonly List<ContentBase> _content = new List<ContentBase>();
        private readonly ConcurrentDictionary<Guid, IContentFactory> _contentFactories = new ConcurrentDictionary<Guid, IContentFactory>();

        private readonly Subject<Unit> _contentCleared = new Subject<Unit>();
        private readonly Subject<ContentBase> _contentCreated = new Subject<ContentBase>();
        private readonly Subject<IContentFactoryMeta> _factoryAdded = new Subject<IContentFactoryMeta>();


        public IReadOnlyDictionary<Guid, IContentFactory> FactoryInfo => _contentFactories;

        public IReadOnlyList<ContentBase> Content => _content;

        public IObservable<IContentFactoryMeta> FactoryAdded => _factoryAdded;

        public IObservable<Unit> ContentCleared => _contentCleared;

        public IObservable<ContentBase> ContentCreated => _contentCreated;



        public void AddFactory(IContentFactory factory)
        {
            if (!_contentFactories.TryAdd(factory.Id, factory))
                throw new Exception();
            
            _factoryAdded.OnNext(factory);
        }

        public ContentBase Create(Guid factoryId)
        {
            if (!_contentFactories.TryGetValue(factoryId, out var factory))
                throw new Exception();

            var instance = factory.CreateDefaultContentInstance();

            _content.Add(instance);

            _contentCreated.OnNext(instance);

            return instance;
        }

        public void ClearHistory()
        {
            _content.Clear();

            _contentCleared.OnNext(new Unit());
        }
    }
}
