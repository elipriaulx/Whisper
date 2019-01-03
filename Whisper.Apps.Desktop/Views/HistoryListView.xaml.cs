using ReactiveUI;

namespace Whisper.Apps.Desktop.Views
{
    public partial class HistoryListView
    {
        public HistoryListView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, x => x.HistoryList, x => x.HistoryList.ItemsSource));
            });
        }
    }
}