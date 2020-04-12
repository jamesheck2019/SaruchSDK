using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SaruchSDK.JSON;

namespace SaruchSDK
{
    public interface IFolder
    {
        Task<JSON_FolderMetadata> Create(string FolderName);
        Task<bool> Delete();
        Task<JSON_List> List(int OffSet = 1, Utilitiez.SortEnum Sort = Utilitiez.SortEnum.name, Utilitiez.OrderByEnum OrderBy = Utilitiez.OrderByEnum.asc);
        Task<JSON_FolderMetadata> Metadata();
        Task<bool> Move(string DestinationFolderID);
        Task<bool> Rename(string NewName);
        Task<JSON_Upload> Upload(object FileToUpload, Utilitiez.UploadTypes UploadType, string FileName, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default);
        Task<JSON_RemoteUpload> UploadRemotly(List<string> FileUrls);
    }
}