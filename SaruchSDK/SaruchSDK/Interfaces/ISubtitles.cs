using System;
using System.Threading;
using System.Threading.Tasks;
using SaruchSDK.JSON;

namespace SaruchSDK
{
    public interface ISubtitles
    {
        Task<JSON_FolderMetadata> Delete(string DestinationSubtitleID);
        Task<JSON_FolderMetadata> Metadata();
        Task<JSON_FolderMetadata> Metadata2(string DestinationSubtitleID);
        Task<JSON_Upload> Upload(object FileToUpload, Utilitiez.UploadTypes UploadType, Utilitiez.LanguageCodeEnum LanguageCode, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default);
    }
}