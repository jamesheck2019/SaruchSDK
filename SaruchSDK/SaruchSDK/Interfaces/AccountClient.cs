using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaruchSDK.JSON;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static SaruchSDK.Basic;
using static SaruchSDK.Utilitiez;

namespace SaruchSDK
{
    public class AccountClient : IAccount
    {

        public async Task<JSON_UserInfo> UserInfo()
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Get , new pUri("auth/me").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_UserInfo>(result, JSONhandler) : throw ShowError(result);
        }

        public async Task<JSON_StorageInfo> StorageInfo()
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Get, new pUri("dashboard").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_StorageInfo>(result, JSONhandler) : throw ShowError(result);
        }

        public async Task<JSON_GetToken> RenewToken()
        {
            HttpContent streamContent = new StringContent(JsonConvert.SerializeObject(new { guard = "user" }), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await RequestAsync(HttpMethod.Post, new pUri("auth/refresh").ToString(), streamContent);
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_GetToken>(result, JSONhandler) : throw ShowError(result);
        }

        public async Task<JSON_GetConversionSetings> GetConversionSettings()
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Get, new pUri("conversion/settings").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_GetConversionSetings>(result, JSONhandler) : throw ShowError(result);
        }

        public async Task<bool> ChangeConversionSettings(Dictionary<ResolutionEnum, bool> Resolutions)
        {
            HttpContent streamContent = new StringContent(JsonConvert.SerializeObject(new { resolutions = (from r in Resolutions where r.Value == true select r.Key).Cast<List<int>>().ToList() }), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await RequestAsync(HttpMethod.Put, new pUri("conversion/settings").ToString(), streamContent);
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JObject.Parse(result).SelectToken("message").ToString() == "Embed settings was updated." : throw ShowError(result);
        }

    }
}
