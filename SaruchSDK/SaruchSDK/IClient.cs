namespace SaruchSDK
{
    public interface IClient
    {
        IFolder Folder(string FolderID);
        IRoot Root();
        IVideo Video(string VideoID);
        IRemoteUpload RemoteUpload();
        IAccount Account();

    }
}