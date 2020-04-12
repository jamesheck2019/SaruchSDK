using System;
using System.Threading;
using System.Threading.Tasks;
using SaruchSDK.JSON;

namespace SaruchSDK
{
    public interface IVideo
    {
        ISubtitles Subtitles { get; }

        Task<bool> Delete();
        Task Download(string StreamVideoUrl, string FileSavePath, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default);
        Task<JSON_GetDownloadUrl> GetDownloadUrl();
        Task<bool> Monetize(bool SetMonetize);
        Task<bool> Move(string DestinationFolderID);
        Task<bool> Privacy(Utilitiez.PrivacyEnum SetPrivacy);
        Task<bool> Rename(string NewName);
        Task<bool> SetVideoCategory(Utilitiez.VideoCategoryEnum Category);
        Task<bool> ThumbnailDelete();
        Task<JSON_SetVideoThumbnail> ThumbnailSet(object FileToUpload, Utilitiez.UploadTypes UploadType, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default);
    }
}