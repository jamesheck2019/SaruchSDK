using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaruchSDK.JSON;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static SaruchSDK.Basic;
using static SaruchSDK.Utilitiez;

namespace SaruchSDK
{
    public class RemoteUploadClient : IRemoteUpload
    {

        public async Task<JSON_RemoteUploadQueue> ListQueue(int OffSet = 1)
        {
            var parameters = new Dictionary<string, string>
            {
                { "page", OffSet.ToString() },
                { "per_page", "50" },
                { "sort", JsonConvert.SerializeObject(new { where = SortEnum.created_at.ToString(), by = OrderByEnum.desc.ToString() }) }
            };
            HttpResponseMessage response = await RequestAsync(HttpMethod.Get,Build(new pUri("remote-uploads").ToString(), parameters));
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_RemoteUploadQueue>(result, JSONhandler) : throw ShowError(result);
        }

        public async Task<JSON_RemoteUploadQueue> Search(string Keyword, int OffSet = 1)
        {
            var parameters = new Dictionary<string, string>
            {
                { "search", Keyword },
                { "page", OffSet.ToString() },
                { "per_page", "50" }
            };
            HttpResponseMessage response = await RequestAsync(HttpMethod.Get, Build(new pUri("remote-uploads").ToString(), parameters));
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_RemoteUploadQueue>(result, JSONhandler) : throw ShowError(result);
        }

        public async Task<JSON_UploadRemoteStatus> GetStatus(string JobID)
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Get, new pUri($"remote-uploads/{JobID}").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_UploadRemoteStatus>(result, JSONhandler) : throw ShowError(result);
        }

        public async Task<bool> Delete(string JobID)
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Delete , new pUri($"remote-uploads/{JobID}").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JObject.Parse(result).SelectToken("message").ToString() == "Remote upload deleted." : throw ShowError(result);
        }

        public async Task<string> Clear(UploadRemoteClearEnum ClearType)
        {
            var streamEncoded = (new Dictionary<string, string>() { { "name", ClearType.ToString() } }).EncodedContent();
            HttpResponseMessage response = await RequestAsync(HttpMethod.Delete , new pUri("remote-uploads").ToString(), streamEncoded);
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JObject.Parse(result).SelectToken("message").ToString() : throw ShowError(result);
        }

    }
}
