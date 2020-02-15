Imports SaruchSDK.JSON

Public Interface IVideo

    ''' <summary>
    ''' video Subtitles
    ''' </summary>
    ReadOnly Property Subtitles() As ISubtitles

    ''' <summary>
    ''' get video download url [direct]
    ''' </summary>
    Function GetDownloadUrl() As Task(Of JSON_GetDownloadUrl)
    ''' <summary>
    ''' download a video
    ''' </summary>
    ''' <param name="StreamVideoUrl">the stream url, get it from here: [GetDownloadUrl]</param>
    ''' <param name="FileSaveDir">local download path [c:\\]</param>
    ''' <param name="DestinationVideoLink">https://saruch.co/videos/xxxx</param>
    ''' <param name="ReportCls">downloading progress tracking</param>
    ''' <param name="token">downloading progress cancellation</param>
    Function Download(StreamVideoUrl As String, FileSaveDir As String, DestinationVideoLink As String, Optional ReportCls As IProgress(Of ReportStatus) = Nothing, Optional token As Threading.CancellationToken = Nothing) As Task
    ''' <summary>
    ''' change video Category
    ''' </summary>
    Function Category(SetCategory As utilitiez.VideoCategoryEnum) As Task(Of Boolean)
    ''' <summary>
    ''' rename exists video
    ''' </summary>
    Function Rename(NewName As String) As Task(Of Boolean)
    ''' <summary>
    ''' move video to another folder
    ''' </summary>
    Function Move(DestinationFolderID As String) As Task(Of Boolean)
    ''' <summary>
    ''' delete a video
    ''' </summary>
    Function Delete() As Task(Of Boolean)
    ''' <summary>
    ''' search for a video in your account
    ''' </summary>
    ''' <param name="OffSet">video count to skip</param>
    Function Search(Keywords As String, Optional OffSet As Integer = 1) As Task(Of JSON_Search)
    ''' <summary>
    ''' change video Privacy
    ''' </summary>
    Function Privacy(SetPrivacy As utilitiez.PrivacyEnum) As Task(Of Boolean)
    ''' <summary>
    ''' Monetize a video
    ''' </summary>
    Function Monetize(SetMonetize As Boolean) As Task(Of Boolean)
    ''' <summary>
    ''' set video thumbnail
    ''' </summary>
    ''' <param name="FileToUpload">the thumbnail image object, can be video local path, memorystream , bytearray</param>
    ''' <param name="UploadType">the type of thumbnail image object</param>
    ''' <param name="ReportCls">uploading progress tracking</param>
    ''' <param name="token">uploading progress cancellation</param>
    Function ThumbnailSet(FileToUpload As Object, UploadType As utilitiez.UploadTypes, Optional ReportCls As IProgress(Of ReportStatus) = Nothing, Optional token As Threading.CancellationToken = Nothing) As Task(Of JSON_SetVideoThumbnail)
    ''' <summary>
    ''' delete video thumbnail
    ''' </summary>
    Function ThumbnailDelete() As Task(Of Boolean)
End Interface
