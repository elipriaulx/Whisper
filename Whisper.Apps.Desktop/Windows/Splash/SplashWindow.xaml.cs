using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Whisper.Apps.Common.Models.Initialisation;

namespace Whisper.Apps.Desktop.Windows.Splash
{
    public partial class SplashWindow : IDisposable, INotifyPropertyChanged
    {
        private readonly IDisposable _disposables;

        public string ProgressText { get; private set; }

        public SplashWindow(IObservable<InitialisationUpdate> updates)
        {
            InitializeComponent();
            DataContext = this;

            _disposables = updates.ObserveOnDispatcher().Do(x =>
            {
                ProgressText = $"{x.Stage}: {x.Message}";
                OnPropertyChanged(nameof(ProgressText));
            }).Subscribe();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
