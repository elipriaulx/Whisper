using System;
using System.Windows;
using System.Windows.Input;
using ReactiveUI;
using Whisper.Apps.Desktop.Controls.Helpers;

namespace Whisper.Apps.Desktop.Controls
{
    public class LustdWindow : Window
    {
        private static readonly DependencyPropertyKey MinimiseTrayCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MinimiseToTrayCommand), typeof(ICommand), typeof(LustdWindow), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty MinimiseTrayCommandProperty = MinimiseTrayCommandPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey AllowTrayPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AllowTray), typeof(bool), typeof(LustdWindow), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty AllowTrayProperty = AllowTrayPropertyKey.DependencyProperty;

        public static readonly DependencyProperty AllowTopmostProperty = DependencyProperty.Register(nameof(AllowTopmost), typeof(bool), typeof(LustdWindow), new UIPropertyMetadata(false));

        public static readonly DependencyProperty TitleContentProperty = DependencyProperty.Register(nameof(TitleContent), typeof(FrameworkElement), typeof(LustdWindow), new UIPropertyMetadata(null));

        public static readonly DependencyProperty ActionContentProperty = DependencyProperty.Register(nameof(ActionContent), typeof(FrameworkElement), typeof(LustdWindow), new UIPropertyMetadata(null));

        public static readonly DependencyProperty AllowTitleInteractionProperty = DependencyProperty.Register(nameof(AllowTitleInteraction), typeof(bool), typeof(LustdWindow), new UIPropertyMetadata(false));

        private Action _notificatioNTryMinimiseAction;

        static LustdWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LustdWindow), new FrameworkPropertyMetadata(typeof(LustdWindow)));
        }

        public LustdWindow()
        {
            SourceInitialized += (sender, e) => LustdWindowHelpers.OnSourceInitialized(this);

            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (sender, e) => { SystemCommands.CloseWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (sender, e) => { SystemCommands.MaximizeWindow(this); }, (sender, e) => { e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip; }));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (sender, e) => { SystemCommands.MinimizeWindow(this); }, (sender, e) => { e.CanExecute = ResizeMode != ResizeMode.NoResize; }));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (sender, e) => { SystemCommands.RestoreWindow(this); }, (sender, e) => { e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip; }));

            MinimiseToTrayCommand = ReactiveCommand.Create(() => { _notificatioNTryMinimiseAction?.Invoke(); });
        }

        public ICommand MinimiseToTrayCommand
        {
            get => (ICommand) GetValue(MinimiseTrayCommandProperty);
            protected set => SetValue(MinimiseTrayCommandPropertyKey, value);
        }

        public bool AllowTray
        {
            get => (bool) GetValue(AllowTrayProperty);
            protected set => SetValue(AllowTrayPropertyKey, value);
        }

        public bool AllowTopmost
        {
            get => (bool) GetValue(AllowTopmostProperty);
            set => SetValue(AllowTopmostProperty, value);
        }

        public FrameworkElement TitleContent
        {
            get => (FrameworkElement) GetValue(TitleContentProperty);
            set => SetValue(TitleContentProperty, value);
        }

        public FrameworkElement ActionContent
        {
            get => (FrameworkElement) GetValue(ActionContentProperty);
            set => SetValue(ActionContentProperty, value);
        }

        public bool AllowTitleInteraction
        {
            get => (bool) GetValue(AllowTitleInteractionProperty);
            set => SetValue(AllowTitleInteractionProperty, value);
        }

        [Obsolete("Pending something better...")]
        public void SetNotificationTrayMinimiseAction(Action action = null)
        {
            // TODO: Implement a better mechanism.
            AllowTray = action != null;
            _notificatioNTryMinimiseAction = action;
        }
    }
}
