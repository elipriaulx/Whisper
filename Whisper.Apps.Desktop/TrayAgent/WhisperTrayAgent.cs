using System;
using System.Drawing;
using System.Windows.Forms;
using Whisper.Apps.Desktop.Controls;
using Whisper.Apps.Desktop.Windows.Settings;

namespace Whisper.Apps.Desktop.TrayAgent
{
    public class WhisperTrayAgent : IDisposable
    {
        private readonly LustdWindow _shellWindow;
        private readonly SettingsWindowManager _settingsManager;
        private readonly NotifyIcon _noticon;

        public WhisperTrayAgent(LustdWindow shellWindow, SettingsWindowManager settingsManager)
        {
            _shellWindow = shellWindow;
            _settingsManager = settingsManager;

            _noticon = new NotifyIcon
            {
                BalloonTipText = "Whisper is still running. Shhh!",
                BalloonTipTitle = "Whisper",
                Text = "Whisper",
                Icon = new Icon("WhisperIcon.ico")
            };

            _noticon.DoubleClick += (sender, e) => { RestoreApplication(); };

            _noticon.ContextMenu = new ContextMenu(new[]
            {
                new MenuItem("Restore", (sender, e) => { RestoreApplication(); }),
                new MenuItem("-"),
                new MenuItem("Settings", (sender, e) => { ShowSettings(); }),
                new MenuItem("Exit", (sender, e) => { CloseApplication(); })
            });

            shellWindow.SetNotificationTrayMinimiseAction(() => MinimiseApplication());

            shellWindow.IsVisibleChanged += (sender, e) =>
            {
                var isVisible = (bool) e.NewValue;

                if (isVisible)
                    DismissTray();
            };
        }

        public void Dispose()
        {
            _noticon?.Dispose();
        }

        private void DismissTray()
        {
            _noticon.Visible = false;
        }

        private void RestoreApplication()
        {
            DismissTray();
            _shellWindow.Show();
        }

        private void MinimiseApplication()
        {
            _shellWindow.Hide();
            _noticon.Visible = true;
        }

        private void ShowSettings()
        {
            RestoreApplication();
            _settingsManager.ShowSettingsWindow();
        }

        private void CloseApplication()
        {
            _shellWindow.Close();
        }
    }
}
