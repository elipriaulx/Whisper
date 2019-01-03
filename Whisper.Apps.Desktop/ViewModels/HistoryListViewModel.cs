using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using Whisper.Core.Services;

namespace Whisper.Apps.Desktop.ViewModels
{
    public class HistoryListViewModel : ReactiveObject, IDisposable
    {
        private readonly IDisposable _subscriptions;

        public HistoryListViewModel(IGeneratorService generator, IClipboardService clipboardService)
        {
            _subscriptions = new CompositeDisposable
            {
                generator.ContentCleared.ObserveOnDispatcher().Do(x => { HistoryList.Clear(); }).Subscribe(),
                generator.ContentCreated.ObserveOnDispatcher().Do(x =>
                {
                    var vm = new HistoryListItemViewModel(x, clipboardService);
                    HistoryList.Insert(0, vm);
                }).Subscribe()
            };

            ClearHistoryCommand = ReactiveCommand.Create(generator.ClearHistory);
        }
        
        public ICommand ClearHistoryCommand { get; }

        public ObservableCollection<HistoryListItemViewModel> HistoryList { get; } = new ObservableCollection<HistoryListItemViewModel>();

        public void Dispose()
        {
            _subscriptions?.Dispose();
        }
    }
}