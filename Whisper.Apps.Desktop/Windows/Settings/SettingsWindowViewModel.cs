using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Whisper.Apps.Desktop.Windows.Settings.ViewModels;

namespace Whisper.Apps.Desktop.Windows.Settings
{
    public class SettingsWindowViewModel : ReactiveObject
    {
        public SettingsWindowViewModel(IEnumerable<SettingsPageViewModelBase> pages)
        {
            var pageList = pages.ToList();

            Pages = pageList;
            CurrentPage = pageList.FirstOrDefault();

            ApplyCommand = ReactiveCommand.Create(ApplyChanges);
            ResetCommand = ReactiveCommand.Create(CancelChanges);

            CancelChanges();
        }

        public ICommand ApplyCommand { get; }

        public ICommand ResetCommand { get; }

        [Reactive]
        public IEnumerable<SettingsPageViewModelBase> Pages { get; protected set; }

        [Reactive]
        public SettingsPageViewModelBase CurrentPage { get; set; }

        private void ApplyChanges()
        {
            if (Pages == null)
                return;

            foreach (var p in Pages) p.ApplyConfigChanges();
        }

        private void CancelChanges()
        {
            if (Pages == null)
                return;

            foreach (var p in Pages) p.CancelConfigChanges();
        }
    }
}
