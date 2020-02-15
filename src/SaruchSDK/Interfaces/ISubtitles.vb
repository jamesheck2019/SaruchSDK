Imports SaruchSDK.JSON

Public Interface ISubtitles

    ''' <summary>
    ''' set video subtitle
    ''' </summary>
    ''' <param name="FileToUpload">the subtitle object, can be video local path, memorystream , bytearray</param>
    ''' <param name="UploadType">the type of subtitle object</param>
    ''' <param name="LanguageCode"></param>
    ''' <param name="ReportCls">uploading progress tracking</param>
    ''' <param name="token">uploading progress cancellation</param>
    Function Upload(FileToUpload As Object, UploadType As utilitiez.UploadTypes, LanguageCode As utilitiez.LanguageCodeEnum, Optional ReportCls As IProgress(Of ReportStatus) = Nothing, Optional token As Threading.CancellationToken = Nothing) As Task(Of JSON_Upload)
    ''' <summary>
    ''' get all video subtitles metadata
    ''' </summary>
    Function Metadatas() As Task(Of JSON_FolderMetadata)
    ''' <summary>
    ''' get subtitle metagata
    ''' </summary>
    Function Metadata(DestinationSubtitleID As String) As Task(Of JSON_FolderMetadata)
    ''' <summary>
    ''' delete a video subtitle
    ''' </summary>
    Function Delete(DestinationSubtitleID As String) As Task(Of JSON_FolderMetadata)


End Interface
