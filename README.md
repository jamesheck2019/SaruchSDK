## SaruchSDK

`Download:`[https://github.com/jamesheck2019/SaruchSDK/releases](https://github.com/jamesheck2019/SaruchSDK/releases)<br>
`NuGet:`
[![NuGet](https://img.shields.io/nuget/v/DeQmaTech.SaruchSDK.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/DeQmaTech.SaruchSDK)<br>
`Help:`
[https://github.com/jamesheck2019/SaruchSDK/wiki](https://github.com/jamesheck2019/SaruchSDK/wiki)<br>

# Features
* Assemblies for .NET 4.5.2 and .NET Standard 2.0 and .NET Core 2.1
* Just one external reference (Newtonsoft.Json)
* Easy installation using NuGet
* Upload/Download tracking support
* Proxy Support
* Upload/Download cancellation support

# List of functions:
**Authentication**
1. OneHourToken
1. SignUp

**Account**
1. UserInfo
1. StorageInfo
1. RenewToken
1. GetConversionSettings
1. ChangeConversionSettings

**Folder**
1. List
1. Metadata
1. Create
1. Rename
1. Delete
1. Move
1. UploadLocalVideo
1. UploadRemoteVideo

**RemoteUpload**
1. Queue
1. Search
1. Status
1. Delete

**Subtitles**
1. Clear
1. Upload
1. Metadatas
1. Metadata
1. Delete

**Video**
1. GetDownloadUrl
1. Download
1. Category
1. Rename
1. Move
1. Delete
1. Search
1. Privacy
1. Monetize
1. ThumbnailSet
1. ThumbnailDelete


# CodeMap:
![codemap](https://i.postimg.cc/0QLF0L4V/sa-codemap.png)


# Code simple:
```vb.net
'register new account
Await SaruchSDK.Authentication.SignUp("nik", "mail", "pass")
'first get auth token
Dim tokn = Await SaruchSDK.Authentication.OneHourToken("your_email", "your_password")
''set proxy and connection options
Dim con As New SaruchSDK.ConnectionSettings With {.CloseConnection = True, .TimeOut = TimeSpan.FromMinutes(30), .Proxy = New SaruchSDK.ProxyConfig With {.SetProxy = True, .ProxyIP = "127.0.0.1", .ProxyPort = 8888, .ProxyUsername = "user", .ProxyPassword = "pass"}}
''set api client
Dim CLNT As SaruchSDK.IClient = New SaruchSDK.SClient(tokn.access_token, con)

''folder
Await CLNT.Folder("folder_id").Create("new folder")
Await CLNT.Folder("folder_id").Delete
Await CLNT.Folder("folder_id").List(1, SortEnum.name, OrderByEnum.asc)
Await CLNT.Folder("folder_id").Metadata
Await CLNT.Folder("folder_id").Move("folder_id")
Await CLNT.Folder("folder_id").Rename("new name")
Await CLNT.Folder("folder_id").UploadRemoteVideo(New List(Of String) From {{"https://domain.com/video.mp4"}, {"http://domain.com/video2.mp4"}})
Dim cts As New Threading.CancellationTokenSource()
Dim _ReportCls As New Progress(Of SaruchSDK.ReportStatus)(Sub(ReportClass As SaruchSDK.ReportStatus) Console.WriteLine(String.Format("{0} - {1}% - {2}", String.Format("{0}/{1}", (ReportClass.BytesTransferred), (ReportClass.TotalBytes)), CInt(ReportClass.ProgressPercentage), ReportClass.TextStatus)))
Await CLNT.Folder("folder_id").UploadLocalVideo("c:\video.mp4", UploadTypes.FilePath, "video.mp4", _ReportCls, cts.Token)

''video
Await CLNT.Video("video_id").Category(VideoCategoryEnum.Regular)
Await CLNT.Video("video_id").Delete
Dim dUrl = Await CLNT.Video("video_id").GetDownloadUrl
Await CLNT.Video("video_id").Download(dUrl.video.Downloads(0).DownloadUrl, "c:\downloads", "https://saruch.co/videos/xxxx", _ReportCls, cts.Token)
Await CLNT.Video("video_id").Monetize(True)
Await CLNT.Video("video_id").Move("folder_id")
Await CLNT.Video("video_id").Privacy(PrivacyEnum.Private)
Await CLNT.Video("video_id").Rename("new video name")
Await CLNT.Video("video_id").Search("emy", 1)
Await CLNT.Video("video_id").ThumbnailSet("c:\myPhoto.jpg", UploadTypes.FilePath, _ReportCls, cts.Token)
Await CLNT.Video("video_id").ThumbnailDelete
Await CLNT.Video("video_id").Subtitles.Delete("subtitle_id")
Await CLNT.Video("video_id").Subtitles.Metadata("subtitle_id")
Await CLNT.Video("video_id").Subtitles.Metadatas
Await CLNT.Video("video_id").Subtitles.Upload("c:\sub.srt", UploadTypes.FilePath, LanguageCodeEnum.Greek, _ReportCls, cts.Token)

''RemoteUpload
Await CLNT.RemoteUpload.Clear(UploadRemoteClearEnum.done)
Await CLNT.RemoteUpload.Delete("job_id")
Await CLNT.RemoteUpload.Queue(5)
Await CLNT.RemoteUpload.Search("domain.com", 1)
Await CLNT.RemoteUpload.Status("job_id")

''Account
Await CLNT.Account.ChangeConversionSettings(New Dictionary(Of ResolutionEnum, Boolean) From {{ResolutionEnum.x144, True}, {ResolutionEnum.x360, True}})
Await CLNT.Account.GetConversionSettings
Await CLNT.Account.RenewToken
Await CLNT.Account.StorageInfo
Await CLNT.Account.UserInfo

```
