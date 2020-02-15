Imports SaruchSDK.utilitiez
Imports Newtonsoft.Json
Imports SaruchSDK.JSON

Public Class SubtitlesClient
    Implements ISubtitles


    Private Property VideoID As String

    Sub New(VideoID As String)
        Me.VideoID = VideoID
    End Sub


#Region "Upload"
    Public Async Function _UploadLocal(FileToUpload As Object, UploadType As UploadTypes, LanguageCode As LanguageCodeEnum, Optional ReportCls As IProgress(Of ReportStatus) = Nothing, Optional token As Threading.CancellationToken = Nothing) As Task(Of JSON_Upload) Implements ISubtitles.Upload
        VideoID.IsRequired()
        Dim filesize As Integer = 0
        If ReportCls Is Nothing Then ReportCls = New Progress(Of ReportStatus)
        ReportCls.Report(New ReportStatus With {.Finished = False, .TextStatus = "Initializing..."})
        Try
            Dim progressHandler As New Net.Http.Handlers.ProgressMessageHandler(New HCHandler)
            AddHandler progressHandler.HttpSendProgress, (Function(sender, e)
                                                              ReportCls.Report(New ReportStatus With {.ProgressPercentage = e.ProgressPercentage, .BytesTransferred = e.BytesTransferred, .TotalBytes = If(e.TotalBytes Is Nothing, 0, e.TotalBytes), .TextStatus = "Uploading..."})
                                                          End Function)
            Dim localHttpClient As New HttpClient(progressHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.Method = Net.Http.HttpMethod.Post
            ''''''''''''''''''''''''''''''''''
            Dim MultipartsformData = New Net.Http.MultipartFormDataContent()
            Dim streamContent As Net.Http.HttpContent
            Select Case UploadType
                Case UploadTypes.FilePath
                    streamContent = New Net.Http.StreamContent(New IO.FileStream(FileToUpload, IO.FileMode.Open, IO.FileAccess.Read))
                    filesize = New System.IO.FileInfo(FileToUpload).Length
                Case UploadTypes.Stream
                    Dim strm = CType(FileToUpload, IO.Stream)
                    streamContent = New Net.Http.StreamContent(strm)
                    filesize = strm.Length
                Case UploadTypes.BytesArry
                    Dim byt = CType(FileToUpload, Byte())
                    streamContent = New Net.Http.StreamContent(New IO.MemoryStream(byt))
                    filesize = byt.Length
            End Select
            streamContent.Headers.Clear()
            streamContent.Headers.ContentDisposition = New System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") With {.Name = """subtitle"""}
            ''''''''''''''''''''''''''
            MultipartsformData.Add(New Net.Http.StringContent(utilitiez.stringValueOf(LanguageCode)), """language_code""")
            MultipartsformData.Add(New Net.Http.StringContent(True), """force""")
            MultipartsformData.Add(streamContent)
            HtpReqMessage.Content = MultipartsformData

            HtpReqMessage.RequestUri = New Uri(String.Format("https//api.saruch.co/videos/{0}/subtitles", VideoID))
            '''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
            Using ResPonse As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(False)
                Dim result As String = Await ResPonse.Content.ReadAsStringAsync()

                token.ThrowIfCancellationRequested()
                If ResPonse.StatusCode = Net.HttpStatusCode.OK Then
                    ReportCls.Report(New ReportStatus With {.Finished = True, .TextStatus = "Subtitle Uploaded successfully"})
                    Return JsonConvert.DeserializeObject(Of JSON_Upload)(result, JSONhandler)
                Else
                    ShowError(result)
                    ReportCls.Report(New ReportStatus With {.Finished = True, .TextStatus = String.Format("The request returned with HTTP status code {0}", ResPonse.ReasonPhrase)})
                End If
            End Using
        Catch ex As Exception
            ReportCls.Report(New ReportStatus With {.Finished = True})
            If ex.Message.ToString.ToLower.Contains("a task was canceled") Then
                ReportCls.Report(New ReportStatus With {.TextStatus = ex.Message})
            Else
                Throw ExceptionCls.CreateException(ex.Message, Nothing)
            End If
        End Try
    End Function
#End Region

#Region "GetsubtitleMetadata"
    Public Async Function _Metadata() As Task(Of JSON_FolderMetadata) Implements ISubtitles.Metadatas
        VideoID.IsRequired()
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri(String.Format("videos/{0}/subtitles", VideoID))
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_GetFolderMetadata)(result, JSONhandler).folder
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "GetsubtitleMetadata2"
    Public Async Function _Metadata2(DestinationSubtitleID As String) As Task(Of JSON_FolderMetadata) Implements ISubtitles.Metadata
        VideoID.IsRequired()
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri(String.Format("videos/{0}/subtitles/{1}", VideoID, DestinationSubtitleID))
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_GetFolderMetadata)(result, JSONhandler).folder
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "Delete"
    Public Async Function _Delete(DestinationSubtitleID As String) As Task(Of JSON_FolderMetadata) Implements ISubtitles.Delete
        VideoID.IsRequired()
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri(String.Format("videos/{0}/subtitles/{1}", VideoID, DestinationSubtitleID))
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.DeleteAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_GetFolderMetadata)(result, JSONhandler).folder
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

End Class
