using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whisper.Apps.Desktop.Windows.Settings.ViewModels
{
    public class SettingsPageApplicationViewModel : SettingsPageViewModelBase
    {
        public override string Name => "Application";
        public override string Description => "General application settings.";

        public override void ApplyConfigChanges()
        {

        }

        public override void CancelConfigChanges()
        {

        }
    }
}
