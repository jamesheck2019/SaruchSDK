Imports Newtonsoft.Json
Imports SaruchSDK.JSON

Module Base

    Public Property APIbase As String = "https://api.saruch.co/"
    Public Property m_TimeOut As System.TimeSpan = Threading.Timeout.InfiniteTimeSpan
    Public Property m_CloseConnection As Boolean = True
    Friend Property ConnectionSetting As ConnectionSettings
    Public Property JSONhandler As New Newtonsoft.Json.JsonSerializerSettings() With {.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore, .NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore}
    Public Property AccessToken() As String


    Private _proxy As ProxyConfig
    Public Property m_proxy As ProxyConfig
        Get
            Return If(_proxy, New ProxyConfig)
        End Get
        Set(value As ProxyConfig)
            _proxy = value
        End Set
    End Property


    Public Class HCHandler
        Inherits Net.Http.HttpClientHandler
        Sub New()
            MyBase.New()
            If m_proxy.SetProxy Then
                MaxRequestContentBufferSize = 1 * 1024 * 1024
                Proxy = New Net.WebProxy(String.Format("http://{0}:{1}", m_proxy.ProxyIP, m_proxy.ProxyPort), True, Nothing, New Net.NetworkCredential(m_proxy.ProxyUsername, m_proxy.ProxyPassword))
                UseProxy = m_proxy.SetProxy
            End If
        End Sub
    End Class

    Public Class pUri
        Inherits Uri
        Sub New(Action As String, Optional Parameters As Dictionary(Of String, String) = Nothing)
            MyBase.New(APIbase + Action + If(Parameters Is Nothing, Nothing, utilitiez.AsQueryString(Parameters)))
        End Sub
    End Class

    Public Class HttpClient
        Inherits Net.Http.HttpClient
        Sub New(HCHandler As HCHandler)
            MyBase.New(HCHandler)
            DefaultRequestHeaders.UserAgent.ParseAdd("SaruchSDK")
            DefaultRequestHeaders.ConnectionClose = m_CloseConnection
            Timeout = m_TimeOut
            DefaultRequestHeaders.Authorization = New Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken)
        End Sub
        Sub New(progressHandler As Net.Http.Handlers.ProgressMessageHandler)
            MyBase.New(progressHandler)
            DefaultRequestHeaders.UserAgent.ParseAdd("SaruchSDK")
            DefaultRequestHeaders.ConnectionClose = m_CloseConnection
            Timeout = m_TimeOut
            DefaultRequestHeaders.Authorization = New Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken)
        End Sub
    End Class

    <Runtime.CompilerServices.Extension()>
    Public Function ShowError(result As String)
        Dim errorInfo = JsonConvert.DeserializeObject(Of JSON_Error)(result, JSONhandler)
        Throw CType(ExceptionCls.CreateException(errorInfo._ErrorMessage, errorInfo._ErrorMessage), SaruchException)
    End Function


    <Runtime.CompilerServices.Extension()>
    Public Function EncodedContent(parametersDictionary As Dictionary(Of String, String)) As Net.Http.FormUrlEncodedContent
        Return New Net.Http.FormUrlEncodedContent(parametersDictionary)
    End Function

    <Runtime.CompilerServices.Extension()>
    Public Sub IsRequired(id As String)
        If id Is Nothing Then Throw ExceptionCls.CreateException("ID is required", 404)
    End Sub
End Module
