using System;
using System.Net;
using System.Threading.Tasks;
using static SaruchSDK.Basic;

namespace SaruchSDK
{
    public class SClient : IClient
    {

        public SClient(string Access_Token, ConnectionSettings Settings = null)
        {
            ServicePointManager.Expect100Continue = true; ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            AccessToken = Access_Token;

            if (Settings == null)
            {
                m_proxy = null;
            }
            else
            {
                m_proxy = Settings.Proxy;
                m_CloseConnection = Settings.CloseConnection ?? true;
                m_TimeOut = Settings.TimeOut ?? TimeSpan.FromMinutes(60);
            }
            RenewTokenInLOOP(TimeSpan.FromMinutes(60));
        }

        public async Task RenewTokenInLOOP(TimeSpan interval)
        {
            while (true)
            {
                await Task.Delay(interval);
                AccountClient clnt = new AccountClient();
                var rslt = await clnt.RenewToken();
                AccessToken = rslt.access_token;
            }
        }

        public IFolder Folder(string FolderID) => new FolderClient(FolderID);
        public IRoot Root() => new RootClient();
        public IVideo Video(string VideoID) => new VideoClient(VideoID);
        public IRemoteUpload RemoteUpload() => new RemoteUploadClient();
        public IAccount Account() => new AccountClient();

    }
}
