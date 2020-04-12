using Newtonsoft.Json;
using SaruchSDK.JSON;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static SaruchSDK.Basic;
using static SaruchSDK.Utilitiez;

namespace SaruchSDK
{
    public class RootClient : IRoot
    {

        public async Task<JSON_List> List(int OffSet = 1, SortEnum Sort = SortEnum.name, OrderByEnum OrderBy = OrderByEnum.asc)
        {
            var clint = new SClient(AccessToken, ConnectionSetting);
            return await clint.Folder(string.Empty).List(OffSet, Sort, OrderBy);
        }

        public async Task<JSON_FolderMetadata> CreateFolder(string FolderName)
        {
            var clint = new SClient(AccessToken, ConnectionSetting);
            return await clint.Folder(string.Empty).Create(FolderName);
        }

        public async Task<JSON_Upload> Upload(object FileToUpload, UploadTypes UploadType, string FileName, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default)
        {
            var clint = new SClient(AccessToken, ConnectionSetting);
            return await clint.Folder(string.Empty).Upload(FileToUpload, UploadType, FileName, ReportCls, token);
        }

        public async Task<JSON_Search> Search(string Keywords, int OffSet = 1)
        {
            var parameters = new Dictionary<string, string>() { { "search", Keywords }, { "page", OffSet.ToString() } };
            HttpResponseMessage response = await RequestAsync(HttpMethod.Get, Build("video-manager", parameters));
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_Search>(result, JSONhandler) : throw ShowError(result);
        }

        public string RootID => string.Empty;
    }
}
