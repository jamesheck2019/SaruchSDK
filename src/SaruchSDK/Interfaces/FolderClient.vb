Imports Newtonsoft.Json
Imports SaruchSDK.JSON
Imports SaruchSDK.utilitiez

Public Class FolderClient
    Implements IFolder

    Private Property FolderID As String

    Sub New(FolderID As String)
        Me.FolderID = FolderID
    End Sub
    Sub New()
    End Sub

#Region "List"
    Public Async Function GET_List(Optional OffSet As Integer = 1, Optional Sort As SortEnum = SortEnum.name, Optional OrderBy As OrderByEnum = OrderByEnum.asc) As Task(Of JSON_List) Implements IFolder.List
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("video-manager", New Dictionary(Of String, String) From {{"per_page", 50}, {"folder_id", FolderID}, {"page", OffSet}, {"sort", JsonConvert.SerializeObject(New With {.where = Sort.ToString, .by = OrderBy.ToString})}})
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_List)(result, JSONhandler)
                Else
                    result.ShowError()
                End If
            End Using
        End Using
    End Function
#End Region

#Region "Metadata"
    Public Async Function _Metadata() As Task(Of JSON_FolderMetadata) Implements IFolder.Metadata
        FolderID.IsRequired()
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri(String.Format("folders/{0}", FolderID))
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_GetFolderMetadata)(result, JSONhandler).folder
                Else
                    result.ShowError()
                End If
            End Using
        End Using
    End Function
#End Region

#Region "CreateNewFolder"
    Public Async Function _CreateNewFolder(FolderName As String) As Task(Of JSON_FolderMetadata) Implements IFolder.Create
        Dim parameters = New Dictionary(Of String, String) From {{"parent_id", FolderID}, {"name", FolderName}}

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New pUri("folders")
            HtpReqMessage.Method = Net.Http.HttpMethod.Post
            HtpReqMessage.Content = parameters.EncodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_FolderMetadata)(result, JSONhandler)
                Else
                    result.ShowError()
                End If
            End Using
        End Using
    End Function
#End Region

#Region "RenameFolder"
    Public Async Function _RenameFolder(NewName As String) As Task(Of Boolean) Implements IFolder.Rename
        FolderID.IsRequired()
        Dim parameters = New Dictionary(Of String, String) From {{"name", NewName}}

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New pUri(String.Format("folders/{0}", FolderID))
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

#Region "MoveFolder"
    Public Async Function _MoveFolder(DestinationFolderID As String) As Task(Of Boolean) Implements IFolder.Move
        FolderID.IsRequired()
        Dim parameters = New Dictionary(Of String, String) From {{"folder_id", DestinationFolderID}}

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New pUri(String.Format("folders/{0}", FolderID))
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

#Region "DeleteFolder"
    Public Async Function _DeleteFolder() As Task(Of Boolean) Implements IFolder.Delete
        FolderID.IsRequired()
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri(String.Format("folders/{0}", FolderID))
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.DeleteAsync(RequestUri).ConfigureAwait(False)

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

#Region "UploadFile"

#Region "GetUploadUrl"
    Private Async Function _GetUploadUrl(FileName As String, FileSize As String) As Task(Of JSON_GetUploadUrl)
        If IO.Path.GetExtension(FileName).ToLower = ".wmv" Then Throw New System.Exception("Mime type not allowed")

        Dim parameters = New Dictionary(Of String, String)
        parameters.Add("name", FileName)
        parameters.Add("size", FileSize)
        parameters.Add("mime_type", "video/mp4") 'Web.MimeMapping.GetMimeMapping(FileName))
        parameters.Add("folder_id", FolderID)

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New pUri("videos")
            HtpReqMessage.Method = Net.Http.HttpMethod.Post
            HtpReqMessage.Content = parameters.EncodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_GetUploadUrl)(result, JSONhandler)
                Else
                    result.ShowError()
                End If
            End Using
        End Using
    End Function
#End Region

    Public Async Function Get_UploadLocal(FileToUpload As Object, UploadType As UploadTypes, FileName As String, Optional ReportCls As IProgress(Of ReportStatus) = Nothing, Optional token As Threading.CancellationToken = Nothing) As Task(Of JSON_Upload) Implements IFolder.UploadLocalVideo
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
            streamContent.Headers.ContentDisposition = New System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") With {.Name = """video""", .FileName = """" + FileName + """"}
            ''''''''''''''''''''''''''
            Dim uploadUrl = Await _GetUploadUrl(FileName, filesize)
            MultipartsformData.Add(New Net.Http.StringContent(uploadUrl.video.id), """video_id""")
            MultipartsformData.Add(New Net.Http.StringContent("resumeable"), """upload_type""")
            MultipartsformData.Add(streamContent)
            HtpReqMessage.Content = MultipartsformData

            HtpReqMessage.RequestUri = New Uri(uploadUrl.server)
            '''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
            Using ResPonse As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(False)
                Dim result As String = Await ResPonse.Content.ReadAsStringAsync()

                token.ThrowIfCancellationRequested()
                If ResPonse.StatusCode = Net.HttpStatusCode.OK Then
                    ReportCls.Report(New ReportStatus With {.Finished = True, .TextStatus = String.Format("[{0}] Uploaded successfully", FileName)})
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

#Region "RemoteUpload"
    Public Async Function GET_RemoteUpload(FileUrls As List(Of String)) As Task(Of JSON_RemoteUpload) Implements IFolder.UploadRemoteVideo
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New pUri("remote-uploads")
            HtpReqMessage.Method = Net.Http.HttpMethod.Post
            Dim JSONobj = New With {.urls = String.Join("\", FileUrls)}
            Dim streamContent As Net.Http.HttpContent = New Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(JSONobj), System.Text.Encoding.UTF8, "application/json")
            HtpReqMessage.Content = streamContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_RemoteUpload)(result, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region


End Class
