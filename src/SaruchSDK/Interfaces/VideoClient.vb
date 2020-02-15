Imports Newtonsoft.Json
Imports SaruchSDK.JSON
Imports SaruchSDK.utilitiez

Public Class VideoClient
    Implements IVideo


    Private Property VideoID As String

    Sub New(VideoID As String)
        Me.VideoID = VideoID
    End Sub


    Public ReadOnly Property Subtitles As ISubtitles Implements IVideo.Subtitles
        Get
            Return New SubtitlesClient(VideoID)
        End Get
    End Property


#Region "GetDownloadUrl"
    Public Async Function GetDownloadUrl() As Task(Of JSON_GetDownloadUrl) Implements IVideo.GetDownloadUrl
        VideoID.IsRequired()
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri(String.Format("videos/{0}/stream", VideoID))
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Dim fin = JsonConvert.DeserializeObject(Of JSON_GetDownloadUrl)(result, JSONhandler)
                    fin.video.Downloads.ForEach(Sub(onz As JSON_GetDownloadUrlVideoSource) onz.DownloadUrl = String.Concat(onz.file, "&de=", fin.de, "&en=", fin.en).Replace("stream.saruch.co", "storage.saruch.co"))
                    Return fin
                Else
                    result.ShowError()
                End If
            End Using
        End Using
    End Function
#End Region

#Region "DownloadFile"
    Public Async Function GET_DownloadFile(StreamVideoUrl As String, FileSavePath As String, DestinationVideoLink As String, Optional ReportCls As IProgress(Of ReportStatus) = Nothing, Optional token As Threading.CancellationToken = Nothing) As Task Implements IVideo.Download
        If ReportCls Is Nothing Then ReportCls = New Progress(Of ReportStatus)
        ReportCls.Report(New ReportStatus With {.Finished = False, .TextStatus = "Initializing..."})
        Try
            Dim hand As New HCHandler With {.MaxRequestContentBufferSize = 1 * 1024 * 1024, .AllowAutoRedirect = True, .MaxAutomaticRedirections = 6}
            Dim progressHandler As New Net.Http.Handlers.ProgressMessageHandler(hand)
            AddHandler progressHandler.HttpReceiveProgress, (Function(sender, e)
                                                                 ReportCls.Report(New ReportStatus With {.ProgressPercentage = e.ProgressPercentage, .BytesTransferred = e.BytesTransferred, .TotalBytes = If(e.TotalBytes Is Nothing, 0, e.TotalBytes), .TextStatus = "Downloading..."})
                                                             End Function)
            Dim localHttpClient As New HttpClient(progressHandler)
            localHttpClient.DefaultRequestHeaders.Referrer = New Uri(DestinationVideoLink)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage() With {.Method = Net.Http.HttpMethod.Get, .RequestUri = New Uri(StreamVideoUrl)}
            '''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
            Using ResPonse As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(False)
                If ResPonse.IsSuccessStatusCode Then
                    ReportCls.Report(New ReportStatus With {.Finished = True, .TextStatus = (String.Format("[{0}] Downloaded successfully.", IO.Path.GetFileName(FileSavePath)))})
                Else
                    ReportCls.Report(New ReportStatus With {.Finished = True, .TextStatus = ((String.Format("Error code: {0}", ResPonse.StatusCode)))})
                End If

                ResPonse.EnsureSuccessStatusCode()
                Dim stream_ = Await ResPonse.Content.ReadAsStreamAsync()
                Using fileStream = New IO.FileStream(FileSavePath, IO.FileMode.Append, IO.FileAccess.Write)
                    stream_.CopyTo(fileStream)
                End Using
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

#Region "ChangeVideoCategory"
    Public Async Function _ChangeVideoCategory(SetCategory As VideoCategoryEnum) As Task(Of Boolean) Implements IVideo.Category
        VideoID.IsRequired()
        Dim parameters = New Dictionary(Of String, String) From {{"video_type_id", CInt(SetCategory)}}

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New pUri(String.Format("videos/{0}", VideoID))
            HtpReqMessage.Method = Net.Http.HttpMethod.Put
            HtpReqMessage.Content = parameters.EncodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return True
                Else
                    Dim result As String = Await response.Content.ReadAsStringAsync()
                    result.ShowError()
                End If
            End Using
        End Using
    End Function
#End Region

#Region "RenameVideo"
    Public Async Function _RenameFile(NewName As String) As Task(Of Boolean) Implements IVideo.Rename
        VideoID.IsRequired()
        Dim parameters = New Dictionary(Of String, String) From {{"name", NewName}}

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New pUri(String.Format("videos/{0}", VideoID))
            HtpReqMessage.Method = Net.Http.HttpMethod.Put
            HtpReqMessage.Content = parameters.EncodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return True
                Else
                    Dim result As String = Await response.Content.ReadAsStringAsync()
                    result.ShowError()
                End If
            End Using
        End Using
    End Function
#End Region

#Region "DeleteVideo"
    Public Async Function GET_DeleteFile() As Task(Of Boolean) Implements IVideo.Delete
        VideoID.IsRequired()
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri(String.Format("videos/{0}", VideoID))
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.DeleteAsync(RequestUri).ConfigureAwait(False)

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return True
                Else
                    Dim result As String = Await response.Content.ReadAsStringAsync()
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "MoveVideo"
    Public Async Function GET_MoveFile(DestinationFolderID As String) As Task(Of Boolean) Implements IVideo.Move
        VideoID.IsRequired()
        Dim parameters = New Dictionary(Of String, String) From {{"folder_id", DestinationFolderID}}

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New pUri(String.Format("videos/{0}", VideoID))
            HtpReqMessage.Method = Net.Http.HttpMethod.Put
            HtpReqMessage.Content = parameters.EncodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return True
                Else
                    Dim result As String = Await response.Content.ReadAsStringAsync()
                    result.ShowError()
                End If
            End Using
        End Using
    End Function
#End Region

#Region "Search"
    Public Async Function GET_Search(Keywords As String, Optional OffSet As Integer = 1) As Task(Of JSON_Search) Implements IVideo.Search
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("video-manager", New Dictionary(Of String, String) From {{"search", Keywords}, {"page", OffSet}})
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_Search)(result, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region


#Region "ChangePrivacy"
    Public Async Function GET_ChangePrivacy(SetPrivacy As utilitiez.PrivacyEnum) As Task(Of Boolean) Implements IVideo.Privacy
        VideoID.IsRequired()
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New pUri(String.Format("videos/{0}", VideoID))
            HtpReqMessage.Method = Net.Http.HttpMethod.Put
            Dim JSONobj = New With {.public = Convert.ToBoolean(SetPrivacy)}
            Dim streamContent As Net.Http.HttpContent = New Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(JSONobj), System.Text.Encoding.UTF8, "application/json")
            HtpReqMessage.Content = streamContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return True
                Else
                    Dim result As String = Await response.Content.ReadAsStringAsync()
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "ChangeMonetize"
    Public Async Function GET_ChangeMonetize(SetMonetize As Boolean) As Task(Of Boolean) Implements IVideo.Monetize
        VideoID.IsRequired()
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New pUri(String.Format("videos/{0}", VideoID))
            HtpReqMessage.Method = Net.Http.HttpMethod.Put
            Dim JSONobj = New With {.monetize = SetMonetize}
            Dim streamContent As Net.Http.HttpContent = New Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(JSONobj), System.Text.Encoding.UTF8, "application/json")
            HtpReqMessage.Content = streamContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return True
                Else
                    Dim result As String = Await response.Content.ReadAsStringAsync()
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region


#Region "ThumbnailSet"
    Public Async Function _ThumbnailSet(FileToUpload As Object, UploadType As UploadTypes, Optional ReportCls As IProgress(Of ReportStatus) = Nothing, Optional token As Threading.CancellationToken = Nothing) As Task(Of JSON_SetVideoThumbnail) Implements IVideo.ThumbnailSet
        VideoID.IsRequired()
        If ReportCls Is Nothing Then ReportCls = New Progress(Of ReportStatus)
        ReportCls.Report(New ReportStatus With {.Finished = False, .TextStatus = "Initializing..."})
        Try
            Dim progressHandler As New Net.Http.Handlers.ProgressMessageHandler(New HCHandler)
            AddHandler progressHandler.HttpSendProgress, (Function(sender, e)
                                                              ReportCls.Report(New ReportStatus With {.ProgressPercentage = e.ProgressPercentage, .BytesTransferred = e.BytesTransferred, .TotalBytes = If(e.TotalBytes Is Nothing, 0, e.TotalBytes), .TextStatus = "Uploading..."})
                                                          End Function)
            Dim localHttpClient As New HttpClient(progressHandler)
            Dim HtpReqMessage As Net.Http.HttpRequestMessage = New Net.Http.HttpRequestMessage()
            HtpReqMessage.Method = Net.Http.HttpMethod.Post
            ''''''''''''''''''''''''''''''''''
            Dim MultipartsformData = New Net.Http.MultipartFormDataContent()
            Dim streamContent As Net.Http.HttpContent
            Select Case UploadType
                Case UploadTypes.FilePath
                    streamContent = New Net.Http.StreamContent(New IO.FileStream(FileToUpload, IO.FileMode.Open, IO.FileAccess.Read))
                Case UploadTypes.Stream
                    streamContent = New Net.Http.StreamContent(CType(FileToUpload, IO.Stream))
                Case UploadTypes.BytesArry
                    streamContent = New Net.Http.StreamContent(New IO.MemoryStream(CType(FileToUpload, Byte())))
            End Select
            streamContent.Headers.Clear()
            streamContent.Headers.ContentDisposition = New System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") With {.Name = """thumbnail""", .FileName = """blob"""}
            MultipartsformData.Add(streamContent)
            ''''''''''''''''''''''''''
            HtpReqMessage.Content = MultipartsformData

            HtpReqMessage.RequestUri = New pUri(String.Format("videos/{0}/thumbnail", VideoID))
            '''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
            Using ResPonse As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(False)
                Dim result As String = Await ResPonse.Content.ReadAsStringAsync()

                token.ThrowIfCancellationRequested()
                If ResPonse.StatusCode = Net.HttpStatusCode.OK Then
                    ReportCls.Report(New ReportStatus With {.Finished = True, .TextStatus = ("Thumbnail Uploaded successfully")})
                    Return JsonConvert.DeserializeObject(Of JSON_SetVideoThumbnail)(result, JSONhandler)
                Else
                    ReportCls.Report(New ReportStatus With {.Finished = True, .TextStatus = String.Format("The request returned with HTTP status code {0}", ResPonse.ReasonPhrase)})
                    Dim errorInfo = JsonConvert.DeserializeObject(Of JSON_Error)(result, JSONhandler)
                    Throw CType(ExceptionCls.CreateException(errorInfo._ErrorMessage, ResPonse.StatusCode), SaruchException)
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

#Region "ThumbnailDelete"
    Public Async Function _ThumbnailDelete() As Task(Of Boolean) Implements IVideo.ThumbnailDelete
        VideoID.IsRequired()
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri(String.Format("videos/{0}/thumbnail", VideoID))
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.DeleteAsync(RequestUri).ConfigureAwait(False)

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return True
                Else
                    Dim result As String = Await response.Content.ReadAsStringAsync()
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

    '#Region "GetVideoThumbnail"
    '    Public Async Function GET_GetVideoThumbnail(DestinationVideoID As String, Optional ReportCls As IProgress(Of ReportStatus) = Nothing, Optional token As Threading.CancellationToken = Nothing) As Task(Of IO.Stream) Implements IThumbnail.GetImg
    '        If ReportCls Is Nothing Then ReportCls = New Progress(Of ReportStatus)
    '        ReportCls.Report(New ReportStatus With {.Finished = False, .TextStatus = "Initializing..."})
    '        Try
    '            Dim progressHandler As New Net.Http.Handlers.ProgressMessageHandler(HCHandler)
    '            AddHandler progressHandler.HttpReceiveProgress, (Function(sender, e)
    '                                                                 ReportCls.Report(New ReportStatus With {.ProgressPercentage = e.ProgressPercentage, .BytesTransferred = e.BytesTransferred, .TotalBytes = If(e.TotalBytes Is Nothing, 0, e.TotalBytes), .TextStatus = "Downloading..."})
    '                                                             End Function)
    '            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '            Dim localHttpClient As New Net.Http.HttpClient(progressHandler)
    '            localHttpClient.DefaultRequestHeaders.Authorization = New Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken)
    '            localHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("SaruchSDK") '' UserAgent
    '            localHttpClient.DefaultRequestHeaders.ConnectionClose = m_CloseConnection
    '            localHttpClient.Timeout = m_TimeOut

    '            Dim HtpReqMessage As New Net.Http.HttpRequestMessage() With {.Method = Net.Http.HttpMethod.Get, .RequestUri = New Uri(String.Format("https://storage.saruch.co/videos/{0}/thumbnail", DestinationVideoID))}
    '            '''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
    '            Dim ResPonse As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(HtpReqMessage.RequestUri, Net.Http.HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(False)
    '            token.ThrowIfCancellationRequested()
    '            If ResPonse.IsSuccessStatusCode Then
    '                ReportCls.Report(New ReportStatus With {.Finished = True, .TextStatus = ("File Downloaded successfully.")})
    '            Else
    '                ReportCls.Report(New ReportStatus With {.Finished = True, .TextStatus = ((String.Format("Error code: {0}", ResPonse.ReasonPhrase)))})
    '            End If

    '            ResPonse.EnsureSuccessStatusCode()
    '            Dim stream_ As IO.Stream = Await ResPonse.Content.ReadAsStreamAsync()
    '            Return stream_
    '        Catch ex As Exception
    '            ReportCls.Report(New ReportStatus With {.Finished = True})
    '            If ex.Message.ToString.ToLower.Contains("a task was canceled") Then
    '                ReportCls.Report(New ReportStatus With {.TextStatus = ex.Message})
    '            Else
    '                Throw CType(ExceptionCls.CreateException(ex.Message, ex.Message), SaruchException)
    '            End If
    '        End Try
    '    End Function
    '#End Region



End Class
