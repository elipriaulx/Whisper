using System.Windows;
using System.Windows.Controls;

namespace Whisper.Apps.Desktop.Assets
{
    public class WindowTitle : ContentControl
    {
        static WindowTitle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowTitle), new FrameworkPropertyMetadata(typeof(WindowTitle)));
        }
    }
}
