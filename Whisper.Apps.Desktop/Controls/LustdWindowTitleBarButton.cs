using System.Windows;
using System.Windows.Controls;

namespace Whisper.Apps.Desktop.Controls
{
    public class LustdWindowTitleBarButton : Button
    {
        static LustdWindowTitleBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LustdWindowTitleBarButton), new FrameworkPropertyMetadata(typeof(LustdWindowTitleBarButton)));
        }
    }
}
