Imports Newtonsoft.Json
Imports SaruchSDK.JSON

Public Class Authentication


#Region "Get_Token"
    Shared Async Function OneHourToken(Email As String, Password As String) As Task(Of JSON_GetToken)
        Net.ServicePointManager.Expect100Continue = True : Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls Or Net.SecurityProtocolType.Tls11 Or Net.SecurityProtocolType.Tls12 Or Net.SecurityProtocolType.Ssl3

        Dim parameters = New Dictionary(Of String, String)
        parameters.Add("email", Email)
        parameters.Add("password", Password)
        Dim encodedContent = New Net.Http.FormUrlEncodedContent(parameters)

        Using localHttpClient As New Net.Http.HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New Uri("https://api.saruch.co/auth/login")
            HtpReqMessage.Method = Net.Http.HttpMethod.Post
            HtpReqMessage.Content = encodedContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Dim userInfo = JsonConvert.DeserializeObject(Of JSON_GetToken)(result)
                    Return userInfo
                Else
                    Dim erorInfo = JsonConvert.DeserializeObject(Of JSON_Error)(result)
                    Throw CType(ExceptionCls.CreateException(erorInfo._ErrorMessage, response.StatusCode), SaruchException) : Exit Function
                End If
            End Using
        End Using
    End Function
#End Region

#Region "Signup"
    Shared Async Function SignUp(Nickname As String, Email As String, Pass As String) As Task(Of JSON_GetToken)
        Net.ServicePointManager.Expect100Continue = True : Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls Or Net.SecurityProtocolType.Tls11 Or Net.SecurityProtocolType.Tls12 Or Net.SecurityProtocolType.Ssl3

        Using localHttpClient As New Net.Http.HttpClient(New HCHandler)
            Dim HtpReqMessage As New Net.Http.HttpRequestMessage()
            HtpReqMessage.RequestUri = New Uri("https://api.saruch.co/auth/register")
            HtpReqMessage.Method = Net.Http.HttpMethod.Post
            Dim JSONobj = New With {.name = Nickname, .email = Email, .password = Pass, .password_confirmation = Pass}
            Dim streamContent As Net.Http.HttpContent = New Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(JSONobj), System.Text.Encoding.UTF8, "application/json")
            HtpReqMessage.Content = streamContent
            Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
                Dim result As String = Await response.Content.ReadAsStringAsync()

                If response.StatusCode = Net.HttpStatusCode.OK Then
                    Return JsonConvert.DeserializeObject(Of JSON_GetToken)(result, New Newtonsoft.Json.JsonSerializerSettings() With {.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore, .NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore})
                Else
                    Dim errorInfo = JsonConvert.DeserializeObject(Of JSON_Error)(result, New Newtonsoft.Json.JsonSerializerSettings() With {.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore, .NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore})
                    Throw CType(ExceptionCls.CreateException(errorInfo._ErrorMessage, response.ReasonPhrase), SaruchException)
                End If
            End Using
        End Using
    End Function
#End Region

End Class