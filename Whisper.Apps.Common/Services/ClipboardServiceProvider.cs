using System.Drawing;
using System.Windows.Forms;
using Whisper.Core.Services;

namespace Whisper.Apps.Common.Services
{
    public class ClipboardServiceProvider : IClipboardService
    {
        public void SetClipboardText(string text)
        {
            Clipboard.SetText(text);
        }

        public void SetClipboardImage(Bitmap imageData)
        {
            Clipboard.SetImage(imageData);
        }

        public void SetClipboardAudio(byte[] audioData)
        {
            Clipboard.SetAudio(audioData);
        }
    }
}
