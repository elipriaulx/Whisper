using System.ComponentModel;

namespace Whisper.Modules.GuidGenerator
{
    public enum GuidFormats
    {
        [Description("00000000000000000000000000000000")]
        MsGuidN,

        [Description("00000000-0000-0000-0000-000000000000")]
        MsGuidD,

        [Description("{00000000-0000-0000-0000-000000000000}")]
        MsGuidB,

        [Description("(00000000-0000-0000-0000-000000000000)")]
        MsGuidP,

        [Description("{0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}")]
        MsGuidX,
    }
}
