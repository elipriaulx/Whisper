using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Whisper.Core.Models.ProjectMeta;
using Whisper.Core.Services;

namespace Whisper.Apps.Common.Services
{
    public class ApplicationInfoServiceProvider : IApplicationInfoService, IApplicationService
    {
        private FileVersionInfo _versionInfo;

        public ApplicationInfoServiceProvider()
        {
           
        }

        public string ProductName => _versionInfo.ProductName;

        public string CopyrightNote => _versionInfo.LegalCopyright;

        public string ProductVersion => _versionInfo.ProductVersion;

        
        public string LicenceUrl => "https://raw.githubusercontent.com/gpriaulx/Whisper/master/LICENSE";

        public string ProjectUrl => null;

        public string SourceUrl => "https://github.com/gpriaulx/Whisper";


        public IEnumerable<IProductInfo> DependencyInformation { get; private set; }


        public class ProductInfo : IProductInfo
        {
            public string ProductName { get; set; }
            public string CopyrightNote { get; set; }
            public string ProductVersion { get; set; }
            public string LicenceUrl { get; set; }
            public string ProjectUrl { get; set; }
            public string SourceUrl { get; set; }
        }

        public void Dispose()
        {
            
        }

        public void Start()
        {
            _versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

            // TODO: Use some sort of build tool or something to populate the DependencyInformation
            DependencyInformation = new IProductInfo[]
            {
                new ProductInfo
                {
                    ProductName = "Fody",
                    LicenceUrl = "https://raw.githubusercontent.com/Fody/Fody/master/License.txt",
                    SourceUrl = "https://github.com/Fody/Fody",
                },
                new ProductInfo
                {
                    ProductName = "Lustd",
                    LicenceUrl = "https://raw.githubusercontent.com/Fody/Fody/master/License.txt",
                    SourceUrl = "https://raw.githubusercontent.com/gpriaulx/Lustd/master/LICENSE",
                },
                new ProductInfo
                {
                    ProductName = "NETStandard.Library",
                    ProjectUrl = "https://dotnet.microsoft.com/",
                    LicenceUrl = "https://raw.githubusercontent.com/dotnet/standard/master/LICENSE.TXT",
                    SourceUrl = "https://github.com/dotnet/standard",
                },
                new ProductInfo
                {
                    ProductName = "ReactiveUI",
                    ProjectUrl = "https://reactiveui.net/",
                    LicenceUrl = "https://raw.githubusercontent.com/reactiveui/ReactiveUI/master/LICENSE",
                    SourceUrl = "https://github.com/reactiveui/reactiveui",
                },
                new ProductInfo
                {
                    ProductName = "ReactiveUI.Fody",
                    ProjectUrl = "https://reactiveui.net/",
                    LicenceUrl = "https://raw.githubusercontent.com/reactiveui/ReactiveUI/master/LICENSE",
                    SourceUrl = "https://github.com/reactiveui/reactiveui",
                },
                new ProductInfo
                {
                    ProductName = "ReactiveUI.WPF",
                    ProjectUrl = "https://reactiveui.net/",
                    LicenceUrl = "https://raw.githubusercontent.com/reactiveui/ReactiveUI/master/LICENSE",
                    SourceUrl = "https://github.com/reactiveui/reactiveui",
                },
                new ProductInfo
                {
                    ProductName = "System.Collections.Immutable",
                    ProjectUrl = "https://dotnet.microsoft.com/",
                    LicenceUrl = "https://raw.githubusercontent.com/dotnet/corefx/master/LICENSE.TXT",
                    SourceUrl = "https://github.com/dotnet/corefx",
                },
                new ProductInfo
                {
                    ProductName = "System.Drawing.Common",
                    ProjectUrl = "https://dotnet.microsoft.com/",
                    LicenceUrl = "https://raw.githubusercontent.com/dotnet/corefx/master/LICENSE.TXT",
                    SourceUrl = "https://github.com/dotnet/corefx",
                },
                new ProductInfo
                {
                    ProductName = "System.Reactive",
                    LicenceUrl = "https://raw.githubusercontent.com/dotnet/reactive/master/LICENSE",
                    SourceUrl = "https://github.com/dotnet/reactive",
                },
                new ProductInfo
                {
                    ProductName = "System.Runtime.CompilerServices.Unsafe",
                    ProjectUrl = "https://dotnet.microsoft.com/",
                    LicenceUrl = "https://raw.githubusercontent.com/dotnet/corefx/master/LICENSE.TXT",
                    SourceUrl = "https://github.com/dotnet/corefx",
                },
                new ProductInfo
                {
                    ProductName = "System.ValueTuple",
                    ProjectUrl = "https://dotnet.microsoft.com/",
                    LicenceUrl = "https://raw.githubusercontent.com/dotnet/corefx/master/LICENSE.TXT",
                    SourceUrl = "https://github.com/dotnet/corefx",
                },
                new ProductInfo
                {
                    ProductName = "UniForm.Core",
                    LicenceUrl = "https://raw.githubusercontent.com/gpriaulx/UniForm/master/LICENSE",
                    SourceUrl = "https://github.com/gpriaulx/UniForm",
                },
                new ProductInfo
                {
                    ProductName = "UniForm.Engine",
                    LicenceUrl = "https://raw.githubusercontent.com/gpriaulx/UniForm/master/LICENSE",
                    SourceUrl = "https://github.com/gpriaulx/UniForm",
                },
                new ProductInfo
                {
                    ProductName = "UniForm.Wpf",
                    LicenceUrl = "https://raw.githubusercontent.com/gpriaulx/UniForm/master/LICENSE",
                    SourceUrl = "https://github.com/gpriaulx/UniForm",
                },
            };
        }

        public void Stop()
        {

        }
    }
}
