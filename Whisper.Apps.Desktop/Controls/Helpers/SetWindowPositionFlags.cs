namespace Whisper.Apps.Desktop.Controls.Helpers
{
    public static class SetWindowPositionFlags
    {
        public const uint AsyncWindowPos = 0x4000;
        public const uint DeferErase = 0x2000;
        public const uint DrawFrame = 0x0020;
        public const uint FrameChanged = 0x0020;
        public const uint HideWindow = 0x0080;
        public const uint NoActivate = 0x0010;
        public const uint NoCopyBits = 0x0100;
        public const uint NoMove = 0x0002;
        public const uint NoOwnerZOrder = 0x0200;
        public const uint NoRedraw = 0x0008;
        public const uint NoReposition = 0x0200;
        public const uint NoSendChanging = 0x0400;
        public const uint NoSize = 0x0001;
        public const uint NoZOrder = 0x0004;
        public const uint ShowWindow = 0x0040;
    }
}
