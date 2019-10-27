## SaruchSDK

`Download:`[https://github.com/jamesheck2019/SaruchSDK/releases](https://github.com/jamesheck2019/SaruchSDK/releases)<br>
`NuGet:`
[![NuGet](https://img.shields.io/nuget/v/DeQmaTech.SaruchSDK.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/DeQmaTech.SaruchSDK)<br>

**Features**
* Assemblies for .NET 4.5.2 and .NET Standard 2.0 and .NET Core 2.1
* Just one external reference (Newtonsoft.Json)
* Easy installation using NuGet
* Upload/Download tracking support
* Proxy Support
* Upload/Download cancellation support

# Functions:
**`IAccount`**
* UserInfo
* StorageInfo
* RenewToken
* GetConversionSetings
* ChangeConversionSetings

**`IFolder`**
* List
* Metadata
* Create
* Rename
* Delete
* Move
* MoveMultiple
* DeleteMultiple

**`IShare`**
* Privacy
* Monetize
* PrivacyMultiple
* MonetizeMultiple

**`ISubtitles`**
* Upload
* Metadata
* Metadata2
* Delete

**`IThumbnail`**
* SetImg
* Delete
* GetImg

**`IVideo`**
* GetDownloadUrl
* Download
* UploadLocal
* UploadRemote
* UploadRemoteQueue
* UploadRemoteSearch
* UploadRemoteStatus
* UploadRemoteDelete
* UploadRemoteClear
* Rename
* Move
* Delete
* MoveMultiple
* DeleteMultiple
* Search




# Code simple:
```vb.net
    Async Sub GetToken()
        Dim tkn = Await SaruchSDK.GetToken.GetToken_OneHour("user", "pass")
        DataGridView1.Rows.Add(tkn.access_token, tkn.expires_in)
    End Sub
```
```vb.net
    Sub SetClient()
        Dim MyClient As SaruchSDK.IClient = New SaruchSDK.SClient("access token")
    End Sub
```
```vb.net
    Sub SetClientWithOptions()
        Dim Optians As New SaruchSDK.ConnectionSettings With {.CloseConnection = True, .TimeOut = TimeSpan.FromMinutes(30), .Proxy = New SaruchSDK.ProxyConfig With {.ProxyIP = "172.0.0.0", .ProxyPort = 80, .ProxyUsername = "myname", .ProxyPassword = "myPass", .SetProxy = True}}
        Dim MyClient As SaruchSDK.IClient = New SaruchSDK.SClient("access token", Optians)
    End Sub
```
```vb.net
    Async Sub GetStorageInfo()
        Dim result = Await MyClient.Account.StorageInfo()
        DataGridView1.Rows.Add(result.amount, result.downloads, result.storage)
    End Sub
```
```vb.net
    Async Sub GetUserInfo()
        Dim result = Await MyClient.Account.UserInfo()
        DataGridView1.Rows.Add(result.email, result.name)
    End Sub
```
```vb.net
    Async Sub ListMyVideosAndFolders()
        Dim result = Await MyClient.Folder.List("folder id / root = null", 1, SortEnum.created_at, OrderByEnum.asc)
        For Each vid In result.folders
            DataGridView1.Rows.Add(vid.name, vid.id, vid.parent_id, vid.updated_at)
        Next
        For Each vid In result.videos.data
            DataGridView1.Rows.Add(vid.name, vid.VideoUrl, vid.size, vid.created_at, vid.dmca, vid.duration, vid.folder_id, vid.public)
        Next
    End Sub
```
```vb.net
    Async Sub DeleteVideoAndFolder()
        Dim result = Await MyClient.Video.Delete("video id")
        Dim resultD = Await MyClient.Folder.Delete("folder id")
    End Sub
```
```vb.net
    Async Sub CreateNewFolder()
        Dim result = Await MyClient.Folder.Create("parent folder id", "new folder name")
    End Sub
```
```vb.net
    Async Sub RenameVideoAndFolder()
        Dim result = Await MyClient.Video.Rename("video id", "new file name")
        Dim resultD = Await MyClient.Folder.Rename("folder id", "new folder name")
    End Sub
```
```vb.net
    Async Sub Upload_Local_WithProgressTracking()
        Dim UploadCancellationToken As New Threading.CancellationTokenSource()
        Dim _ReportCls As New Progress(Of SaruchSDK.ReportStatus)(Sub(ReportClass As SaruchSDK.ReportStatus)
                                                                      Label1.Text = String.Format("{0}/{1}", (ReportClass.BytesTransferred), (ReportClass.TotalBytes))
                                                                      ProgressBar1.Value = CInt(ReportClass.ProgressPercentage)
                                                                      Label2.Text = CStr(ReportClass.TextStatus)
                                                                  End Sub)
        Await MyClient.Video.UploadLocal("J:\DB\myvideo.mp4", UploadTypes.FilePath, "folder id", "myvideo.mp4", _ReportCls, UploadCancellationToken.Token)
    End Sub
```
```vb.net
    Async Sub Download_File_WithProgressTracking()
        Dim DownloadCancellationToken As New Threading.CancellationTokenSource()
        Dim _ReportCls As New Progress(Of SaruchSDK.ReportStatus)(Sub(ReportClass As SaruchSDK.ReportStatus)
                                                                      Label1.Text = String.Format("{0}/{1}", (ReportClass.BytesTransferred), (ReportClass.TotalBytes))
                                                                      ProgressBar1.Value = CInt(ReportClass.ProgressPercentage)
                                                                      Label2.Text = CStr(ReportClass.TextStatus)
                                                                  End Sub)
        Dim FUrl = Await MyClient.Video.GetDownloadUrl("video id")
        Await MyClient.Video.Download(FUrl.video.Downloads(0).DownloadUrl, "J:\DB\", "https://saruch.co/videos/xxxx", _ReportCls, DownloadCancellationToken.Token)
    End Sub
```
```vb.net
    Async Sub SetVideoPublic()
        Dim result = Await MyClient.Share.Privacy("video id", True)
    End Sub
```
```vb.net
    Async Sub GetVideoThumbnail()
        Dim DownloadCancellationToken As New Threading.CancellationTokenSource()
        Dim _ReportCls As New Progress(Of SaruchSDK.ReportStatus)(Sub(ReportClass As SaruchSDK.ReportStatus)
                                                                      Label1.Text = String.Format("{0}/{1}", (ReportClass.BytesTransferred), (ReportClass.TotalBytes))
                                                                      ProgressBar1.Value = CInt(ReportClass.ProgressPercentage)
                                                                      Label2.Text = CStr(ReportClass.TextStatus)
                                                                  End Sub)
        Dim result As IO.Stream = Await MyClient.Thumbnail.GetImg("video id", _ReportCls, DownloadCancellationToken.Token)
    End Sub
```
```vb.net
    Async Sub SetVideoThumbnail()
        Dim UploadCancellationToken As New Threading.CancellationTokenSource()
        Dim _ReportCls As New Progress(Of SaruchSDK.ReportStatus)(Sub(ReportClass As SaruchSDK.ReportStatus)
                                                                      Label1.Text = String.Format("{0}/{1}", (ReportClass.BytesTransferred), (ReportClass.TotalBytes))
                                                                      ProgressBar1.Value = CInt(ReportClass.ProgressPercentage)
                                                                      Label2.Text = CStr(ReportClass.TextStatus)
                                                                  End Sub)
        Await MyClient.Thumbnail.SetImg("J:\DB\myPhoto.jpg", UploadTypes.FilePath, "video id", _ReportCls, UploadCancellationToken.Token)
    End Sub
```
