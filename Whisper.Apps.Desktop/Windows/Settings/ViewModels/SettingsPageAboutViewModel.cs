using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Whisper.Apps.Desktop.Windows.Settings.ComponentViewModels;
using Whisper.Core.Services;

namespace Whisper.Apps.Desktop.Windows.Settings.ViewModels
{
    public class SettingsPageAboutViewModel : SettingsPageViewModelBase
    {
        private readonly IApplicationInfoService _appInfoService;

        public SettingsPageAboutViewModel(IApplicationInfoService appInfoService)
        {
            _appInfoService = appInfoService;

            ProductInfo = new ProductInfoViewModel(appInfoService);
            DependencyInfo = appInfoService.DependencyInformation.OrderBy(x => x.ProductName).Select(x => new ProductInfoViewModel(x)).ToList();

            BrowseCommand = ReactiveCommand.Create<string>(targetUrl =>
            {
                try
                {
                    System.Diagnostics.Process.Start(targetUrl);
                }
                catch
                {
                    // TODO: Log
                }
            });
        }

        public override string Name => "About";
        public override string Description => "About Whisper.";

        [Reactive]
        public ProductInfoViewModel ProductInfo { get; private set; }

        [Reactive]
        public IEnumerable<ProductInfoViewModel> DependencyInfo { get; private set; }

        public ICommand BrowseCommand { get; }

        public override void ApplyConfigChanges()
        {

        }

        public override void CancelConfigChanges()
        {

        }
    }
}
