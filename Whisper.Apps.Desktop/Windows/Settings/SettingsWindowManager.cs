using System;
using System.Windows;

namespace Whisper.Apps.Desktop.Windows.Settings
{
    public class SettingsWindowManager
    {
        private readonly Func<SettingsWindow> _settingsWindowFactory;

        public SettingsWindowManager(Func<SettingsWindow> settingsWindowFactory)
        {
            _settingsWindowFactory = settingsWindowFactory;
        }

        public void ShowSettingsWindow()
        {
            var owner = Application.Current.MainWindow;

            var window = _settingsWindowFactory();

            window.Owner = owner;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            window.ShowDialog();
        }
    }
}
