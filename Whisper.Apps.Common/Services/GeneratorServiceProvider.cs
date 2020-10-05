using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using Whisper.Core.Models.Generation;
using Whisper.Core.Services;

namespace Whisper.Apps.Common.Services
{
    public class GeneratorServiceProvider : IGeneratorService, IApplicationService
    {
        private readonly List<ContentBase> _content = new List<ContentBase>();
        private readonly ConcurrentDictionary<Guid, IContentGenerator> _contentFactories = new ConcurrentDictionary<Guid, IContentGenerator>();

        private readonly Subject<Unit> _contentCleared = new Subject<Unit>();
        private readonly Subject<ContentBase> _contentCreated = new Subject<ContentBase>();
        private readonly Subject<IContentGeneratorMeta> _factoryAdded = new Subject<IContentGeneratorMeta>();


        public IReadOnlyDictionary<Guid, IContentGenerator> GeneratorInfo => _contentFactories;

        public IReadOnlyList<ContentBase> Content => _content;

        public IObservable<IContentGeneratorMeta> GeneratorAdded => _factoryAdded;

        public IObservable<Unit> ContentCleared => _contentCleared;

        public IObservable<ContentBase> ContentCreated => _contentCreated;



        public void AddFactory(IContentGenerator generator)
        {
            if (!_contentFactories.TryAdd(generator.Id, generator))
                throw new Exception();
            
            _factoryAdded.OnNext(generator);
        }

        public ContentBase Create(Guid factoryId, string name = null)
        {
            if (!_contentFactories.TryGetValue(factoryId, out var factory))
                throw new Exception();

            var instance = factory.CreateInstance(name);

            _content.Add(instance);

            _contentCreated.OnNext(instance);

            return instance;
        }

        public ContentBase Create(Guid factoryId, GeneratorConfigurationBase configuration, string name = null)
        {
            if (!_contentFactories.TryGetValue(factoryId, out var factory))
                throw new Exception();

            var instance = factory.CreateInstance(configuration, name);
            
            _content.Add(instance);

            _contentCreated.OnNext(instance);

            return instance;
        }

        public void ClearHistory()
        {
            _content.Clear();

            _contentCleared.OnNext(new Unit());
        }

        public void Dispose()
        {
            
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
