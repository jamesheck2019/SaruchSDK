Imports SaruchSDK.JSON

Public Interface IClient

    ''' <summary>
    ''' folder tasks
    ''' </summary>
    ''' <param name="FolderID">rootid = null</param>
    ReadOnly Property Folder(FolderID As String) As IFolder
    ReadOnly Property Folder As IFolder
    ReadOnly Property Video(VideoID As String) As IVideo
    ReadOnly Property RemoteUpload As IRemoteUpload
    ReadOnly Property Account As IAccount

End Interface
