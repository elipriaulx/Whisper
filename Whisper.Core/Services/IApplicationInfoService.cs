using System.Collections.Generic;
using Whisper.Core.Models.ProjectMeta;

namespace Whisper.Core.Services
{
    public interface IApplicationInfoService : IProductInfo
    {
        IEnumerable<IProductInfo> DependencyInformation { get; }
    }
}
