using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactiveUI;
using Whisper.Apps.Desktop.ViewModels;

namespace Whisper.Apps.Desktop.Views
{
    public partial class HistoryListItemView : ReactiveUserControl<HistoryListItemViewModel>
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
