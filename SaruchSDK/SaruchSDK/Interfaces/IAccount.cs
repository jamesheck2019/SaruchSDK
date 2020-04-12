using System.Collections.Generic;
using System.Threading.Tasks;
using SaruchSDK.JSON;

namespace SaruchSDK
{
    public interface IAccount
    {
        Task<bool> ChangeConversionSettings(Dictionary<Utilitiez.ResolutionEnum, bool> Resolutions);
        Task<JSON_GetConversionSetings> GetConversionSettings();
        Task<JSON_GetToken> RenewToken();
        Task<JSON_StorageInfo> StorageInfo();
        Task<JSON_UserInfo> UserInfo();
    }
}