using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Whisper.Apps.Desktop
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel(ReactiveObject layoutRoot)
        {
            CurrentLayout = layoutRoot;
        }

        [Reactive]
        public ReactiveObject CurrentLayout { get; set; }
    }
}