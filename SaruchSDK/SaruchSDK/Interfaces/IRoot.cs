using System;
using System.Threading;
using System.Threading.Tasks;
using SaruchSDK.JSON;

namespace SaruchSDK
{
    public interface IRoot
    {
        Task<JSON_FolderMetadata> CreateFolder(string FolderName);
        Task<JSON_List> List(int OffSet = 1, Utilitiez.SortEnum Sort = Utilitiez.SortEnum.name, Utilitiez.OrderByEnum OrderBy = Utilitiez.OrderByEnum.asc);
        Task<JSON_Upload> Upload(object FileToUpload, Utilitiez.UploadTypes UploadType, string FileName, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default);
        Task<JSON_Search> Search(string Keywords, int OffSet = 1);
        string RootID { get; }
    }
}