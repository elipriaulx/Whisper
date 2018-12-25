using System;

namespace Whisper.Apps.Desktop.ViewModels
{
    public class HistoryListItemGuidViewModel : HistoryListItemViewModel
    {
        private Guid guid = Guid.NewGuid();

        public override string ContentDescription => "Guid";

        public override string ContentPreview => guid.ToString();

        public override string Icon => System.Net.WebUtility.HtmlDecode("&#xE943;");

        protected override string GetClipboardContents()
        {
            return guid.ToString();
        }
    }
}