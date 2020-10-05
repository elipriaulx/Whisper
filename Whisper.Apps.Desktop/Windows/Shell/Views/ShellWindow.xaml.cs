using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Input;
using ReactiveUI;

namespace Whisper.Apps.Desktop.Windows.Shell
{
    public partial class ShellWindow : IViewFor<ShellWindowViewModel>
    {
        private static readonly DependencyPropertyKey MinimiseTrayCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MinimiseToTrayCommand), typeof(ICommand), typeof(ShellWindow), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty MinimiseTrayCommandProperty = MinimiseTrayCommandPropertyKey.DependencyProperty;


        private Action _notificationTryMinimiseAction;

        private ShellWindowViewModel _viewModel;


        public ShellWindow()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Creator, x => x.CreatorHost.ViewModel).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.HistoryList, x => x.HistoryListHost.ViewModel).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.AlwaysOnTop, x => x.Topmost).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.ShowSettingsCommand, x => x.SettingsButton).DisposeWith(disposables);
            });

            MinimiseToTrayCommand = ReactiveCommand.Create(() => { _notificationTryMinimiseAction?.Invoke(); });
        }

        public ICommand MinimiseToTrayCommand
        {
            get => (ICommand) GetValue(MinimiseTrayCommandProperty);
            protected set => SetValue(MinimiseTrayCommandPropertyKey, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ShellWindowViewModel) value;
        }

        public ShellWindowViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                DataContext = value;
            }
        }

        public void SetNotificationTrayMinimiseAction(Action action = null)
        {
            _notificationTryMinimiseAction = action;
        }
    }
}
