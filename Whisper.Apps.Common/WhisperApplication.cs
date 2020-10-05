using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading.Tasks;
using Whisper.Apps.Common.Models.Initialisation;
using Whisper.Apps.Common.Services;
using Whisper.Core.Models.Generation;
using Whisper.Core.Models.Logging;
using Whisper.Core.Modularity;
using Whisper.Core.Services;

namespace Whisper.Apps.Common
{
    public class WhisperApplication : IDisposable, IComponentRegistry
    {
        public static readonly string LocalAppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Whisper");
        public static readonly string LoggingDirectory = Path.Combine(LocalAppDataDirectory, "Logs");
        public static readonly string ConfigDirectory = Path.Combine(LocalAppDataDirectory, "Config");
        public static readonly string ModuleDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private readonly CompositeDisposable _applicationDisposables = new CompositeDisposable();

        private readonly Subject<InitialisationUpdate> _initialisationProgress = new Subject<InitialisationUpdate>();
        public IObservable<InitialisationUpdate> InitialisationProgress => _initialisationProgress;

        private readonly object _initialisationLock = new object();
        private bool _initialisationRequested = false;

        private readonly List<IApplicationService> _services;

        private readonly ApplicationInfoServiceProvider _appInfoService;
        private readonly LoggingServiceProvider _loggingService;
        private readonly ClipboardServiceProvider _clipboardService;
        private readonly ConfigurationServiceProvider _configService;
        private readonly GeneratorServiceProvider _generatorService;

        public IApplicationInfoService AppInfoService => _appInfoService;
        public ILoggingService LoggingService => _loggingService;
        public IClipboardService ClipboardService => _clipboardService;
        public IConfigurationService ConfigService => _configService;
        public IGeneratorService GeneratorService => _generatorService;

        private readonly ILogger _logger;

        public WhisperApplication()
        {
            if (!Directory.Exists(LoggingDirectory))
                Directory.CreateDirectory(LoggingDirectory);

            if (!Directory.Exists(ConfigDirectory))
                Directory.CreateDirectory(ConfigDirectory);

            _loggingService = new LoggingServiceProvider(LoggingDirectory);
            _appInfoService = new ApplicationInfoServiceProvider();
            _clipboardService = new ClipboardServiceProvider();
            _configService = new ConfigurationServiceProvider(_loggingService);
            _generatorService = new GeneratorServiceProvider();

            _services = new List<IApplicationService>
            {
                _appInfoService,
                _clipboardService,
                _configService,
                _generatorService
            };

            _logger = _loggingService.GetContextualLogger("Initialisation");
        }

        public async Task InitialiseApplication()
        {
            lock (_initialisationLock)
            {
                if (_initialisationRequested)
                    return;

                _initialisationRequested = true;
            }
            
            // Discovering
            List<FileInfo> moduleAssembliesToConsider;
            {
                LogAndRaiseUpdate(InitialisationStages.DiscoveringModules, "Starting module discovery.");

                var directory = new DirectoryInfo(ModuleDirectory);
                moduleAssembliesToConsider = directory.GetFiles("*.dll").Where(x => x.Name.StartsWith("Whisper.Modules.")).ToList();

                LogAndRaiseUpdate(InitialisationStages.DiscoveringModules, $"{moduleAssembliesToConsider.Count} candidate modules discovered to consider.");
            }

            // Services
            {
                LogAndRaiseUpdate(InitialisationStages.StartingServices, $"Preparing {_services.Count} available services.");

                var failedServices = 0;

                foreach (var s in _services)
                {
                    try
                    {
                        s.Start();

                        LogAndRaiseUpdate(InitialisationStages.StartingServices, "Service started.");
                    }
                    catch (Exception ex)
                    {
                        failedServices++;
                        _logger?.Error(ex);
                    }
                }

                LogAndRaiseUpdate(InitialisationStages.StartingServices, $"Services started. {failedServices}/{_services.Count} failed to start.");
            }

            // Configuration
            {
                LogAndRaiseUpdate(InitialisationStages.LoadingConfiguration, "Loading configuration.");

                var configPath = Path.Combine(ConfigDirectory, "application.config");
                _logger?.Debug($"Nominated configuration path: {configPath}");

                _configService.LoadConfiguration(configPath);
                _applicationDisposables.Add(_configService.Updated.Throttle(TimeSpan.FromMilliseconds(500)).Do(x => { _configService.SaveConfiguration(configPath); }).Subscribe());

                LogAndRaiseUpdate(InitialisationStages.LoadingConfiguration, "Configuration loaded.");
            }

            // Modules
            {
                LogAndRaiseUpdate(InitialisationStages.PreparingModules, "Attempting to load modules.");

                var targetInterface = typeof(IWhisperModule);

                var failedModules = 0;

                foreach (var f in moduleAssembliesToConsider)
                {
                    try
                    {
                        var assemblyName = AssemblyName.GetAssemblyName(f.FullName);
                        var loadedAssembly = Assembly.Load(assemblyName);

                        var loadables = loadedAssembly.GetTypes().Where(x => x.IsClass && x.GetInterfaces().Any(y => y == targetInterface)).ToList();

                        foreach (var loadable in loadables)
                        {
                            var module = (IWhisperModule)Activator.CreateInstance(loadable);
                            module.LoadModule(this);
                        }

                        LogAndRaiseUpdate(InitialisationStages.PreparingModules, "Module loaded.");
                    }
                    catch (Exception ex)
                    {
                        failedModules++;
                        _logger?.Error(ex);
                    }
                }

                LogAndRaiseUpdate(InitialisationStages.PreparingModules, $"Module load complete. {failedModules}/{moduleAssembliesToConsider.Count} faild to load.");
            }


            // Finalise
            {
                LogAndRaiseUpdate(InitialisationStages.Finalising, $"Version Summary: {_appInfoService.ProductName}, {_appInfoService.ProductVersion}.");
                LogAndRaiseUpdate(InitialisationStages.Finalising, "Initialisation complete.");
            }
        }


        public void Dispose()
        {
            foreach (var s in _services)
            {
                s.Dispose();
            }

            _initialisationProgress.Dispose();
            _applicationDisposables.Dispose();
        }

        private void LogAndRaiseUpdate(InitialisationStages stage, string message)
        {
            _logger?.Info($"{stage}: {message}");
            _initialisationProgress.OnNext(new InitialisationUpdate(stage, message));
        }

        public void RegisterContentGenerator(ContentGeneratorBase contentGenerator)
        {
            _generatorService.AddFactory(contentGenerator);
        }
    }
}
