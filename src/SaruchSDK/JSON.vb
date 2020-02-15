Imports Newtonsoft.Json
Imports System.ComponentModel

Namespace JSON

    Public Class Response(Of TResult)
        Public ReadOnly Property JSON As Newtonsoft.Json.Linq.JToken
            Get
                Return Newtonsoft.Json.Linq.JToken.Parse(MyClass.ToString)
            End Get
        End Property
        Public Property result As TResult
    End Class

#Region "StatusCode / Error"
    Public Class JSON_Status
        <JsonProperty("status", NullValueHandling:=NullValueHandling.Ignore)> Public Property StatusCode As Integer
    End Class
    Public Class JSON_Error
        <JsonProperty("status", NullValueHandling:=NullValueHandling.Ignore)> Public Property StatusCode As Integer
        <JsonProperty("message", NullValueHandling:=NullValueHandling.Ignore)> Public Property _ErrorMessage As String
    End Class
#End Region

#Region "JSON_GetToken"
    Public Class JSON_GetToken
        Public Property access_token As String
        Public Property token_type As String
        Public Property expires_in As Integer
    End Class
#End Region

#Region "JSON_UserInfo"
    Public Class JSON_UserInfo
        Public Property name As String
        Public Property email As String
    End Class
#End Region

#Region "JSON_StorageInfo"
    Public Class JSON_StorageInfo
        Public Property amount As String
        Public Property videos As Integer
        Public Property storage As long
        Public Property views As Integer
        Public Property downloads As Integer
        Public Property visitors As List(Of Object)
    End Class
#End Region

#Region "JSON_List"
    Public Class JSON_List
        Public Property folders As List(Of JSON_FolderMetadata)
        Public Property videos As JSON_ListVideos
        Public ReadOnly Property HasMore As Boolean
            Get
                Return If(videos.next_page Is Nothing, False, True)
            End Get
        End Property
    End Class
    Public Class JSON_ListVideos
        Public Property data As List(Of JSON_VideoMetadata)
        'Public Property current_page As Integer
        'Public Property first_page As Integer
        Public Property next_page As Object
        'Public Property prev_page As Object
        'Public Property from As Integer
        'Public Property [to] As Integer
        'Public Property per_page As Integer
    End Class
#End Region

#Region "JSON_VideoMetadata"
    Public Class JSON_VideoMetadata
        Public Property id As String
        Public Property folder_id As Object
        Public Property name As String
        Public Property size As String
        Public Property width As String
        Public Property height As String
        Public Property duration As String
        Public Property thumbnail As String
        Public Property monetize As Boolean
        Public Property [public] As Boolean
        Public Property error_code As Object
        Public Property amount As String
        Public Property views As String
        Public Property downloads As String
        Public Property converting As Integer
        Public Property dmca As Integer
        Public Property created_at As String
        Public Property updated_at As String
        Public Property owner As Boolean
        Public Property mime_type As JSON_MimeType
        Public Property subtitles As List(Of Object)
        Public ReadOnly Property VideoUrl As String
            Get
                Return String.Concat("https://saruch.co/videos/", id)
            End Get
        End Property
    End Class
    Public Class JSON_MimeType
        Public Property name As String
        Public Property type As String
        Public Property extension As String
    End Class
#End Region

#Region "JSON_GetFolderMetadata"
    Public Class JSON_GetFolderMetadata
        Public Property folder As JSON_FolderMetadata
    End Class
#End Region

#Region "JSON_FolderMetadata"
    Public Class JSON_FolderMetadata
        Public Property id As String
        Public Property name As String
        Public Property parent_id As Object
        Public Property updated_at As String
        Public Property created_at As String
    End Class
#End Region

#Region "JSON_GetUploadUrl"
    Public Class JSON_GetUploadUrl
        Public Property video As JSON_GetUploadUrlVideo
        Public Property server As String
    End Class
    Public Class JSON_GetUploadUrlVideo
        Public Property id As String
        Public Property folder_id As Object
        Public Property name As String
        Public Property size As String
        Public Property updated_at As String
        Public Property created_at As String
        Public Property subtitles As List(Of Object)
    End Class
#End Region
#Region "JSON_Upload"
    Public Class JSON_Upload
        Public Property video_id As String
        Public Property finished As Integer
        Public Property expires_at As Object
        Public Property updated_at As String
        Public Property created_at As String
        Public Property uploaded As Integer
    End Class
#End Region

#Region "JSON_RemoteUpload"
    Public Class JSON_RemoteUpload
        Public Property message As String
        <JsonProperty("invalid_urls")> Public Property NotAcceptedUrls As List(Of String)
        Public Property remote_uploads As List(Of JSON_RemoteUploadMeta)
        Public ReadOnly Property IsErrorExists As Boolean
            Get
                Return If(Not NotAcceptedUrls.Any(), False, True)
            End Get
        End Property
    End Class
    Public Class JSON_RemoteUploadMeta
        <JsonProperty("id")> Public Property JobID As long
        Public Property video_id As Object
        Public Property url As String
        Public Property status As Boolean
        Public Property done As Boolean
        Public Property failed As Boolean
        Public Property created_at As String
    End Class

#End Region

#Region "JSON_RemoteUploadQueue"
    Public Class JSON_RemoteUploadQueue
        Public Property remote_uploads As JSON_JSON_RemoteUploadQueue_Uploads
        Public ReadOnly Property HasMore As Boolean
            Get
                Return If(remote_uploads.next_page Is Nothing, False, True)
            End Get
        End Property
    End Class
    Public Class JSON_JSON_RemoteUploadQueue_Uploads
        Public Property data As List(Of JSON_RemoteUploadMetadata)
        'Public Property current_page As Integer
        'Public Property first_page As Integer
        Public Property next_page As Object
        'Public Property prev_page As Object
        'Public Property from As Integer
        'Public Property _to As Integer
        'Public Property per_page As Integer
    End Class
    Public Class JSON_RemoteUploadMetadata
        <JsonProperty("id")> Public Property JobID As long
        Public Property video_id As String
        Public Property url As String
        <JsonProperty("status")> Public Property Progress As String
        <Browsable(False)> <Bindable(False)> <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> <EditorBrowsable(EditorBrowsableState.Never)> Public Property done As Boolean
        <Browsable(False)> <Bindable(False)> <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> <EditorBrowsable(EditorBrowsableState.Never)> Public Property failed As Boolean
        Public Property created_at As String
        <Browsable(False)> <Bindable(False)> <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> <EditorBrowsable(EditorBrowsableState.Never)> Public Property video As JSON_JSON_RemoteUploadQueueVideo
        Enum RemoteUploadStatusEnum
            Done
            Faild
            Inprogress
        End Enum
        Public ReadOnly Property UploadStatus As RemoteUploadStatusEnum
            Get
                Select Case True
                    Case done
                        UploadStatus = RemoteUploadStatusEnum.Done
                    Case done = False AndAlso failed = False
                        UploadStatus = RemoteUploadStatusEnum.Inprogress
                    Case failed
                        UploadStatus = RemoteUploadStatusEnum.Faild
                End Select
            End Get
        End Property
        Public ReadOnly Property VideoMetadate As JSON_JSON_RemoteUploadQueueVideo
            Get
                Return If(video Is Nothing, New JSON_JSON_RemoteUploadQueueVideo, video)
            End Get
        End Property
        Public ReadOnly Property VideoUrl As String
            Get
                Return If(Not String.IsNullOrEmpty(video_id), String.Concat("https://saruch.co/videos/", video_id), Nothing)
            End Get
        End Property
    End Class
    Public Class JSON_JSON_RemoteUploadQueueVideo
        Public Property name As String
        Public Property mime_type As Object
        Public Property subtitles() As Object
    End Class
#End Region

#Region "JSON_UploadRemoteStatus"
    Public Class JSON_UploadRemoteStatus
        Public Property remote_upload As JSON_RemoteUploadMetadata
    End Class
#End Region

#Region "JSON_RenameFolder"
    Public Class JSON_RenameFolder
        Public Property JSON As Newtonsoft.Json.Linq.JToken
        Public Property status As Integer
        <JsonProperty("result", NullValueHandling:=NullValueHandling.Ignore)> Public Property Success As Boolean
    End Class
#End Region

#Region "JSON_SetVideoThumbnail"
    Public Class JSON_SetVideoThumbnail
        Public Property message As String
        <JsonProperty("thumbnail")> Public Property ThumbnailUrl As String
    End Class
#End Region

#Region "JSON_GetSplash"
    Public Class JSON_GetSplash
        Public Property Success As Boolean
        Public Property JSON As Newtonsoft.Json.Linq.JToken
        Public Property status As Integer
        <JsonProperty("result", NullValueHandling:=NullValueHandling.Ignore)> Public Property SplashUrl As String
    End Class
#End Region

#Region "JSON_CopyFile"
    Public Class JSON_CopyFile
        Inherits Response(Of JSON_CopyFileResult)
    End Class
    Public Class JSON_CopyFileResult
        Public Property id As String
        Public Property status As Integer
        Public Property name As String
        Public Property size As String
        Public Property sha1 As String
        Public Property content_type As String
        Public Property upload_at As String
        Public Property cstatus As String
    End Class
#End Region

#Region "JSON_GetDownloadUrl"
    Public Class JSON_GetDownloadUrl
        Public Property video As JSON_GetDownloadUrlVideo
        <Browsable(False)> <Bindable(False)> <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> <EditorBrowsable(EditorBrowsableState.Never)> Public Property de As String
        <Browsable(False)> <Bindable(False)> <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> <EditorBrowsable(EditorBrowsableState.Never)> Public Property en As String
    End Class
    Public Class JSON_GetDownloadUrlVideo
        Public Property name As String
        Public Property size As String
        Public Property width As String
        Public Property height As String
        Public Property duration As String
        Public Property thumbnail As String
        <JsonProperty("sources")> Public Property Downloads As List(Of JSON_GetDownloadUrlVideoSource)
    End Class
    Public Class JSON_GetDownloadUrlVideoSource
        <Browsable(False)> <Bindable(False)> <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> <EditorBrowsable(EditorBrowsableState.Never)> Public Property file As String
        <JsonProperty("label")> Public Property Resolution As utilitiez.ResolutionEnum
        <JsonProperty("type")> Public Property Extension As String
        Public Property DownloadUrl As String
    End Class
#End Region

#Region "JSON_Search"
    Public Class JSON_Search
        Public Property videos As Videos
        Public ReadOnly Property HasMore As Boolean
            Get
                Return If(videos.next_page Is Nothing, False, True)
            End Get
        End Property
    End Class
    Public Class Videos
        <JsonProperty("data")> Public Property SearchResults As List(Of JSON_VideoMetadata)
        Public Property next_page As Object
    End Class
#End Region

#Region "JSON_GetConversionSetings"
    Public Class JSON_GetConversionSetings
        <Browsable(False)> <Bindable(False)> <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> <EditorBrowsable(EditorBrowsableState.Never)>
        Public Property resolutions As List(Of Integer)
        Public ReadOnly Property x360 As Boolean
            Get
                Return resolutions.Contains(360)
            End Get
        End Property
        Public ReadOnly Property x480 As Boolean
            Get
                Return resolutions.Contains(480)
            End Get
        End Property
        Public ReadOnly Property x720 As Boolean
            Get
                Return resolutions.Contains(720)
            End Get
        End Property
        Public ReadOnly Property x1080 As Boolean
            Get
                Return resolutions.Contains(1080)
            End Get
        End Property
    End Class
#End Region


End Namespace

