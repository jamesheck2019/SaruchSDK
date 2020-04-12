using Newtonsoft.Json;
using SaruchSDK.JSON;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static SaruchSDK.Basic;

namespace SaruchSDK
{
    public class Authentication
    {

        public static async Task<JSON_GetToken> OneHourToken(string Email, string Password)
        {
            ServicePointManager.Expect100Continue = true; ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            var parameters = new Dictionary<string, string>{{ "email", Email },{ "password", Password }};

            HttpResponseMessage response = await RequestAsync(HttpMethod.Post, "https://api.saruch.co/auth/login", parameters.EncodedContent());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_GetToken>(result, JSONhandler) : throw ShowError(result);
        }

        public static async Task<JSON_GetToken> SignUp(string Nickname, string Email, string Pass)
        {
            ServicePointManager.Expect100Continue = true; ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            var JSONobj = new { name = Nickname, email = Email, password = Pass, password_confirmation = Pass };
            HttpContent streamContent = new StringContent(JsonConvert.SerializeObject(JSONobj), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await RequestAsync(HttpMethod.Post, "https://api.saruch.co/auth/register", streamContent);
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_GetToken>(result, JSONhandler) : throw ShowError(result);
        }

    }
}