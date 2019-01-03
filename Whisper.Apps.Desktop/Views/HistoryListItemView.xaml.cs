using ReactiveUI;
using Whisper.Apps.Desktop.ViewModels;

namespace Whisper.Apps.Desktop.Views
{
    public partial class HistoryListItemView
    {
        public HistoryListItemView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, x => x.ContentDescription, x => x.ContentTitle.Text));
                d(this.OneWayBind(ViewModel, x => x.ContentPreview, x => x.ContentRegion.Text));
                d(this.OneWayBind(ViewModel, x => x.Icon, x => x.IconRegion.Text));
                d(this.BindCommand(ViewModel, x => x.CopyToClipboardCommand, x => x.CopyToClipboardButton));
            });
        }
    }
}
