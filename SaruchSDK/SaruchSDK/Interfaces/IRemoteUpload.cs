using System.Threading.Tasks;
using SaruchSDK.JSON;

namespace SaruchSDK
{
    public interface IRemoteUpload
    {
        Task<string> Clear(Utilitiez.UploadRemoteClearEnum ClearType);
        Task<bool> Delete(string JobID);
        Task<JSON_UploadRemoteStatus> GetStatus(string JobID);
        Task<JSON_RemoteUploadQueue> ListQueue(int OffSet = 1);
        Task<JSON_RemoteUploadQueue> Search(string Keyword, int OffSet = 1);
    }
}