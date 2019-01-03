using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Whisper.Apps.Desktop.Windows.Settings
{
    public class SettingsWindowManager
    {
        public void ShowSettingsWindow()
        {
            var owner = Application.Current.MainWindow;

            var settingsWindow = new SettingsWindow
            {
                Owner = owner,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };


            settingsWindow.ShowDialog();
        }
    }
}
