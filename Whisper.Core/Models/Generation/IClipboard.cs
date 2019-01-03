using System.Drawing;

namespace Whisper.Core.Models.Generation
{
    public interface IClipboard
    {
        void SetClipboardText(string text);

        void SetClipboardImage(Bitmap imageData);

        void SetClipboardAudio(byte[] audioData);
    }
}
