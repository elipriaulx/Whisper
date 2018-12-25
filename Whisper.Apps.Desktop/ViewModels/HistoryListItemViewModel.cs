using System.Windows;
using System.Windows.Input;
using ReactiveUI;

namespace Whisper.Apps.Desktop.ViewModels
{
    public abstract class HistoryListItemViewModel : ReactiveObject
    {
        protected HistoryListItemViewModel()
        {
            CopyToClipboardCommand = ReactiveCommand.Create(CopyToClipboard);
        }

        public void CopyToClipboard()
        {
            var clipboardContent = GetClipboardContents();

            Clipboard.SetText(clipboardContent);
        }

        public ICommand CopyToClipboardCommand { get; }

        public abstract string ContentDescription { get; }

        public abstract string ContentPreview { get; }

        public abstract string Icon { get; }

        protected abstract string GetClipboardContents();
    }
}