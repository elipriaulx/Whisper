using ReactiveUI;

namespace Whisper.Apps.Desktop.Windows.Settings.ViewModels
{
    public abstract class SettingsPageViewModelBase : ReactiveObject
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract void ApplyConfigChanges();

        public abstract void CancelConfigChanges();
    }
}
