using System.Collections.ObjectModel;
using System.Windows.Input;
using ReactiveUI;

namespace Whisper.Apps.Desktop.ViewModels
{
    public class HistoryListViewModel : ReactiveObject
    {
        public HistoryListViewModel()
        {
            AddNewGuidCommand = ReactiveCommand.Create(() =>
            {
                var g = new HistoryListItemGuidViewModel();
                HistoryList.Insert(0, g);
                g.CopyToClipboard();
            });

            AddNewPasswordCommand = ReactiveCommand.Create(() =>
            {
                var p = new HistoryListItemPasswordViewModel();
                HistoryList.Insert(0, p);
                p.CopyToClipboard();
            });
        }

        public ICommand AddNewGuidCommand { get; }
        public ICommand AddNewPasswordCommand { get; }

        public ObservableCollection<HistoryListItemViewModel> HistoryList { get; } = new ObservableCollection<HistoryListItemViewModel>();
    }
}