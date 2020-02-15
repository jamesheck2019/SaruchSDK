Imports SaruchSDK.JSON
Imports Newtonsoft.Json
Imports SaruchSDK.utilitiez

Public Class SClient
    Implements IClient


    Public Sub New(Access_Token As String, Optional Settings As ConnectionSettings = Nothing)
        AccessToken = Access_Token

        ConnectionSetting = Settings
        If Settings Is Nothing Then
            m_proxy = Nothing
        Else
            m_proxy = Settings.Proxy
            m_CloseConnection = If(Settings.CloseConnection, True)
            m_TimeOut = If(Settings.TimeOut = Nothing, TimeSpan.FromMinutes(60), Settings.TimeOut)
        End If
        RenewTokenInLOOP(TimeSpan.FromMinutes(60))
        Net.ServicePointManager.Expect100Continue = True : Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls Or Net.SecurityProtocolType.Tls11 Or Net.SecurityProtocolType.Tls12 Or Net.SecurityProtocolType.Ssl3
    End Sub


    Public Async Function RenewTokenInLOOP(interval As TimeSpan, Optional cancellationToken As Threading.CancellationToken = Nothing) As Task(Of Boolean)
        While True
            Await Task.Delay(interval, cancellationToken)
            Dim clnt As New AccountClient
            Dim rslt = Await clnt.GET_RenewToken()
            AccessToken = rslt.access_token
        End While
        Return True
    End Function


    Public ReadOnly Property Folder(FolderID As String) As IFolder Implements IClient.Folder
        Get
            Return New FolderClient(FolderID)
        End Get
    End Property
    Public ReadOnly Property Folder() As IFolder Implements IClient.Folder
        Get
            Return New FolderClient()
        End Get
    End Property

    Public ReadOnly Property Video(VideoID As String) As IVideo Implements IClient.Video
        Get
            Return New VideoClient(VideoID)
        End Get
    End Property
    Public ReadOnly Property RemoteUpload As IRemoteUpload Implements IClient.RemoteUpload
        Get
            Return New RemoteUploadClient()
        End Get
    End Property
    Public ReadOnly Property Account As IAccount Implements IClient.Account
        Get
            Return New AccountClient()
        End Get
    End Property


End Class
