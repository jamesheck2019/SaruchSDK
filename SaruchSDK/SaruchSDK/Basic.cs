using Newtonsoft.Json;
using SaruchSDK.JSON;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SaruchSDK
{
    internal static class Basic
    {

        public static string APIbase = "https://api.saruch.co/";
        public static JsonSerializerSettings JSONhandler = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore, NullValueHandling = NullValueHandling.Ignore };
        public static string AccessToken = null;
        public static TimeSpan m_TimeOut = System.Threading.Timeout.InfiniteTimeSpan; //' TimeSpan.FromMinutes(60)
        public static bool m_CloseConnection = true;
        public static ConnectionSettings ConnectionSetting = null;


        private static ProxyConfig _proxy;
        public static ProxyConfig m_proxy
        {
            get
            {
                return _proxy ?? new ProxyConfig();
            }
            set
            {
                _proxy = value;
            }
        }

        public class HCHandler : HttpClientHandler
        {
            public HCHandler() : base()
            {
                if (m_proxy.SetProxy)
                {
                    base.MaxRequestContentBufferSize = 1 * 1024 * 1024;
                    base.Proxy = new System.Net.WebProxy($"http://{m_proxy.ProxyIP}:{m_proxy.ProxyPort}", true, null, new System.Net.NetworkCredential(m_proxy.ProxyUsername, m_proxy.ProxyPassword));
                    base.UseProxy = m_proxy.SetProxy;
                }
            }
        }

        public class pUri : Uri
        {
            public pUri(string ApiAction, Dictionary<string, string> Parameters) : base(APIbase + ApiAction + Utilitiez.AsQueryString(Parameters)) { }
            public pUri(string ApiAction) : base(APIbase + ApiAction) { }
        }

        public class HtpClient : HttpClient
        {
            public HtpClient(HCHandler HCHandler) : base(HCHandler)
            {
                base.DefaultRequestHeaders.UserAgent.ParseAdd("SaruchSDK");
                base.DefaultRequestHeaders.ConnectionClose = m_CloseConnection;
                base.Timeout = m_TimeOut;
                base.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                base.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            }
            public HtpClient(System.Net.Http.Handlers.ProgressMessageHandler progressHandler) : base(progressHandler)
            {
                base.DefaultRequestHeaders.UserAgent.ParseAdd("SaruchSDK");
                base.DefaultRequestHeaders.ConnectionClose = m_CloseConnection;
                base.Timeout = m_TimeOut;
                base.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                base.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            }
        }

        public static SaruchException ShowError(string result)
        {
            var errorInfo = JsonConvert.DeserializeObject<JSON_Error>(result, JSONhandler);
            return new SaruchException(errorInfo._ErrorMessage, errorInfo.StatusCode);
        }

        public static FormUrlEncodedContent EncodedContent(this Dictionary<string, string> parametersDictionary)
        {
            return new FormUrlEncodedContent(parametersDictionary);
        }

        public static void IsRequired(this string id)
        {
            if (id == null)
            {
                throw new SaruchException("ID is required", 404);
            }
        }

        public static string Build(string ApiAction, Dictionary<string, string> Parameters)
        {
            return APIbase + ApiAction + Utilitiez.AsQueryString(Parameters);
        }

        public static async Task<HttpResponseMessage> RequestAsync(HttpMethod method, string url)
        {
            using (HtpClient localHtpClient = new HtpClient(new HCHandler { }))
            {
                HttpRequestMessage requ = new HttpRequestMessage(method, new Uri(url));
                HttpResponseMessage response = await localHtpClient.SendAsync(requ, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
                return response;
            }
        }

        public static async Task<HttpResponseMessage> RequestAsync(HttpMethod method, string url, HttpContent content)
        {
            using (HtpClient localHtpClient = new HtpClient(new HCHandler { }))
            {
                HttpRequestMessage requ = new HttpRequestMessage(method, new Uri(url)) { Content = content };
                HttpResponseMessage response = await localHtpClient.SendAsync(requ, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
                return response;
            }
        }

        public static bool Success(this HttpResponseMessage response)
        {
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }


    }
}
