using System.Drawing;
using System.Windows.Forms;
using Whisper.Core.Services;

namespace Whisper.Apps.Common.Services
{
    public class ClipboardServiceProvider : IClipboardService, IApplicationService
    {
        public void SetClipboardText(string text)
        {
            Clipboard.SetText(text);
        }

        public void Dispose()
        {
            
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
