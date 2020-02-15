Imports Newtonsoft.Json
Imports SaruchSDK.JSON

Public Class AccountClient
    Implements IAccount

#Region "UserInfo"
    Public Async Function GET_UserInfo() As Task(Of JSON_UserInfo) Implements IAccount.UserInfo
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("auth/me")
            Using resPonse As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await resPonse.Content.ReadAsStringAsync()

                If resPonse.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_UserInfo)(result, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "StorageInfo"
    Public Async Function GET_StorageInfo() As Task(Of JSON_StorageInfo) Implements IAccount.StorageInfo
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("dashboard")
            Using resPonse As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await resPonse.Content.ReadAsStringAsync()

                If resPonse.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_StorageInfo)(result, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "RenewToken"
    Public Async Function GET_RenewToken() As Task(Of JSON_GetToken) Implements IAccount.RenewToken
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New pUri("auth/refresh")
            HtpReqMessage.Method = Net.Http.HttpMethod.Post
            Dim JSONobj = New With {.guard = "user"}
            Dim streamContent As Net.Http.HttpContent = New Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(JSONobj), System.Text.Encoding.UTF8, "application/json")
            HtpReqMessage.Content = streamContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Dim tkn = JsonConvert.DeserializeObject(Of JSON_GetToken)(result, JSONhandler)
                    AccessToken = tkn.access_token
                    Return tkn
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region



#Region "GetConversionSetings"
    Public Async Function GET_GetConversionSetings() As Task(Of JSON_GetConversionSetings) Implements IAccount.GetConversionSettings
        Using localHttpClient As New HttpClient(New HCHandler)
            Dim RequestUri = New pUri("conversion/settings")
            Using resPonse As Net.Http.HttpResponseMessage = Await localHttpClient.GetAsync(RequestUri).ConfigureAwait(False)
                Dim result As String = Await resPonse.Content.ReadAsStringAsync()

                If resPonse.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_GetConversionSetings)(result, JSONhandler)
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

#Region "ChangeConversionSetings"
    Public Async Function GET_ChangeConversionSetings(Resolutions As Dictionary(Of utilitiez.ResolutionEnum, Boolean)) As Task(Of Boolean) Implements IAccount.ChangeConversionSettings
        'Public Async Function GET_ChangeConversionSetings(x360 As Boolean, x480 As Boolean, x720 As Boolean, x1080 As Boolean) As Task(Of Boolean) Implements IAccount.ChangeConversionSettings
        Using localHttpClient As New HttpClient(New HCHandler)
            Using resPonse As Net.Http.HttpResponseMessage = Await localHttpClient.PutAsync(New pUri("conversion/settings"), New Net.Http.StringContent(JsonConvert.SerializeObject(New With {.resolutions = (From r In Resolutions Where r.Value = True Select r.Key).Cast(Of List(Of Integer))().ToList()}), Text.Encoding.UTF8, "application/json")).ConfigureAwait(False)
                Dim result As String = Await resPonse.Content.ReadAsStringAsync()

                If resPonse.StatusCode = Net.HttpStatusCode.OK Then
                    Return Linq.JObject.Parse(result)("message").ToString = "Embed settings was updated."
                Else
                    ShowError(result)
                End If
            End Using
        End Using
    End Function
#End Region

End Class
