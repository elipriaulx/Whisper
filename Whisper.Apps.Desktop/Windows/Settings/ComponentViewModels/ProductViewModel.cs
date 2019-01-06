using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Whisper.Core.Models.ProjectMeta;

namespace Whisper.Apps.Desktop.Windows.Settings.ComponentViewModels
{
    public class ProductInfoViewModel : ReactiveObject, IProductInfo
    {
        public ProductInfoViewModel()
        {

        }

        public ProductInfoViewModel(IProductInfo productInfo)
        {
            ProductName = productInfo.ProductName;
            CopyrightNote = productInfo.CopyrightNote;
            ProductVersion = productInfo.ProductVersion;
            LicenceUrl = productInfo.LicenceUrl;
            ProjectUrl = productInfo.ProjectUrl;
            SourceUrl = productInfo.SourceUrl;
        }

        [Reactive]
        public string ProductName { get; set; }

        [Reactive]
        public string CopyrightNote { get; set; }

        [Reactive]
        public string ProductVersion { get; set; }

        [Reactive]
        public string LicenceUrl { get; set; }

        [Reactive]
        public string ProjectUrl { get; set; }

        [Reactive]
        public string SourceUrl { get; set; }
    }
}
