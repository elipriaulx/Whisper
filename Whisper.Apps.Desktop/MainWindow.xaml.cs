using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using ReactiveUI;

namespace Whisper.Apps.Desktop
{
    public partial class MainWindow : IViewFor<MainWindowViewModel>
    {
        private NotifyIcon _noticon;
        private bool _terminating;

        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (sender, e) => { SystemCommands.CloseWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (sender, e) => { SystemCommands.MinimizeWindow(this); }, (sender, e) => { e.CanExecute = ResizeMode != ResizeMode.NoResize; }));

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.CurrentLayout, x => x.LayoutHost.ViewModel).DisposeWith(disposables);
            });

            _noticon = new NotifyIcon();
            _noticon.BalloonTipText = "The app has been minimised. Click the tray icon to show.";
            _noticon.BalloonTipTitle = "The App";
            _noticon.Text = "The App";
            _noticon.Icon = new System.Drawing.Icon("WhisperIcon.ico");
            _noticon.DoubleClick += (sender, e) =>
            {
                Show();
            };

            _noticon.ContextMenu = new ContextMenu(new[]
            {
                new MenuItem("Restore", (sender, e) =>
                {
                    Show();
                }),
                new MenuItem("Exit", (sender, e) =>
                {
                    _terminating = true;
                    Close();
                }),
            });

            IsVisibleChanged += OnIsVisibleChanged;
            Closed += OnClose;
            Closing += OnClosing;
        }



        void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (_terminating)
                return;

            cancelEventArgs.Cancel = true;
            Hide();
            _noticon?.ShowBalloonTip(2000);
        }

        void OnClose(object sender, EventArgs eventArgs)
        {
            _noticon.Dispose();
            _noticon = null;
        }
        
        void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon();
        }
        
        void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }

        void ShowTrayIcon(bool show)
        {
            if (_noticon != null)
                _noticon.Visible = show;
        }


        object IViewFor.ViewModel
        {
            get => ViewModel;
            set
            {
                ViewModel = (MainWindowViewModel) value;
                DataContext = value;
            }
        }

        public MainWindowViewModel ViewModel { get; set; }
    }
}