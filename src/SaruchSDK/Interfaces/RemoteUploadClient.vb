Imports Newtonsoft.Json
Imports SaruchSDK.JSON
Imports SaruchSDK.utilitiez

Public Class RemoteUploadClient
    Implements IRemoteUpload



#Region "Queue"
    Public Async Function _Queue(Optional OffSet As Integer = 1) As Task(Of JSON_RemoteUploadQueue) Implements IRemoteUpload.Queue
        Dim parameters = New Dictionary(Of String, String)
        parameters.Add("page", OffSet)
        parameters.Add("per_page", 50)
        parameters.Add("sort", JsonConvert.SerializeObject(New With {.where = utilitiez.SortEnum.created_at.ToString, .by = utilitiez.OrderByEnum.desc.ToString}))

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("remote-uploads", parameters)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_RemoteUploadQueue)(result, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "RemoteUploadSearch"
    Public Async Function _Search(Keyword As String, Optional OffSet As Integer = 1) As Task(Of JSON_RemoteUploadQueue) Implements IRemoteUpload.Search
        Dim parameters = New Dictionary(Of String, String)
        parameters.Add("search", Keyword)
        parameters.Add("page", OffSet)
        parameters.Add("per_page", 50)

        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("remote-uploads", parameters)
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_RemoteUploadQueue)(result, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "UploadRemoteStatus"
    Public Async Function _Status(JobID As String) As Task(Of JSON_UploadRemoteStatus) Implements IRemoteUpload.Status
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri(String.Format("remote-uploads/{0}", JobID))
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_UploadRemoteStatus)(result, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "UploadRemoteDelete"
    Public Async Function _Delete(JobID As String) As Task(Of Boolean) Implements IRemoteUpload.Delete
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri(String.Format("remote-uploads/{0}", JobID))
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.DeleteAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return Linq.JObject.Parse(result)("message").ToString = "Remote upload deleted."
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "UploadRemoteClear"
    Public Async Function _Clear(ClearType As UploadRemoteClearEnum) As Task(Of String) Implements IRemoteUpload.Clear
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage() With {.RequestUri = New pUri("remote-uploads"), .Method = Net.Http.HttpMethod.Delete}
            HtpReqMessage.Content = (New Dictionary(Of String, String) From {{"name", ClearType.ToString}}).EncodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return Linq.JObject.Parse(result)("message").ToString
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region


End Class
