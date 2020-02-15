Imports SaruchSDK.JSON

Public Interface IFolder


    Function List(Optional OffSet As Integer = 1, Optional Sort As utilitiez.SortEnum = utilitiez.SortEnum.name, Optional OrderBy As utilitiez.OrderByEnum = utilitiez.OrderByEnum.asc) As Task(Of JSON_List)
    ''' <summary>
    ''' get folder metadata
    ''' </summary>
    Function Metadata() As Task(Of JSON_FolderMetadata)
    ''' <summary>
    ''' create new folder
    ''' </summary>
    Function Create(FolderName As String) As Task(Of JSON_FolderMetadata)
    ''' <summary>
    ''' rename exists folder
    ''' </summary>
    Function Rename(NewName As String) As Task(Of Boolean)
    ''' <summary>
    ''' delete folder
    ''' </summary>
    Function Delete() As Task(Of Boolean)
    ''' <summary>
    ''' move folder to another folder
    ''' </summary>
    Function Move(DestinationFolderID As String) As Task(Of Boolean)
    ''' <summary>
    ''' upload a video to folder
    ''' </summary>
    ''' <param name="FileToUpload">the video object, can be video local path, memorystream , bytearray</param>
    ''' <param name="UploadType">the type of video object</param>
    ''' <param name="FileName">save video as</param>
    ''' <param name="ReportCls">uploading progress tracking</param>
    ''' <param name="token">uploading progress cancellation</param>
    Function UploadLocalVideo(FileToUpload As Object, UploadType As utilitiez.UploadTypes, FileName As String, Optional ReportCls As IProgress(Of ReportStatus) = Nothing, Optional token As Threading.CancellationToken = Nothing) As Task(Of JSON_Upload)
    ''' <summary>
    ''' upload video from a direct url
    ''' </summary>
    ''' <param name="FileUrls">direct http/https video link</param>
    Function UploadRemoteVideo(FileUrls As List(Of String)) As Task(Of JSON_RemoteUpload)
End Interface
