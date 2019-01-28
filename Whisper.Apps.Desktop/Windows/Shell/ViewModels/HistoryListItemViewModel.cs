using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Whisper.Core.Models.Generation;

namespace Whisper.Apps.Desktop.ViewModels
{
    public class HistoryListItemViewModel : ReactiveObject, IDisposable
    {
        private readonly ContentBase _content;

        protected HistoryListItemViewModel()
        {
        }
        public HistoryListItemViewModel(ContentBase content, IClipboard clipboard) : this()
        {
            _content = content;

            CopyToClipboardCommand = ReactiveCommand.Create(() => { content.SetToClipboard(clipboard); });
            
            Update();
        }

        private void Update()
        {
            ContentDescription = _content.Name;
            ContentPreview = _content.PublicPreviewText;
            Icon = _content.Mdl2Icon;
        }

        public ICommand CopyToClipboardCommand { get; }

        [Reactive]
        public string ContentDescription { get; private set; }

        [Reactive]
        public string ContentPreview { get; private set; }

        [Reactive]
        public string Icon { get; private set; }

        public void Dispose()
        {

        }
    }
}