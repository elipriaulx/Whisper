namespace Whisper.Core.Models.ProjectMeta
{
    public interface IProductInfo
    {
        string ProductName { get; }
        string CopyrightNote { get; }
        string ProductVersion { get; }

        string LicenceUrl { get; }
        string ProjectUrl { get; }
        string SourceUrl { get; }
    }
}
