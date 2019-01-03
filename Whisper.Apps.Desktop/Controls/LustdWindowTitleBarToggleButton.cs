using System.Windows;
using System.Windows.Controls.Primitives;

namespace Whisper.Apps.Desktop.Controls
{
    public class LustdWindowTitleBarToggleButton : ToggleButton
    {
        static LustdWindowTitleBarToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LustdWindowTitleBarToggleButton), new FrameworkPropertyMetadata(typeof(LustdWindowTitleBarToggleButton)));
        }
    }
}
