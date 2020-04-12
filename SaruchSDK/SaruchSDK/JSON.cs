using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.ComponentModel;

namespace SaruchSDK.JSON
{
    public class Response<TResult>
    {
        public TResult result { get; set; }
    }

    public class JSON_Status
    {
        [JsonProperty("status")]public int StatusCode { get; set; }
    }
    public class JSON_Error
    {
        [JsonProperty("status")]public int StatusCode { get; set; }
        [JsonProperty("message")]public string _ErrorMessage { get; set; }
    }

    public class JSON_GetToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    public class JSON_UserInfo
    {
        public string name { get; set; }
        public string email { get; set; }
    }

    public class JSON_StorageInfo
    {
        public string amount { get; set; }
        public int videos { get; set; }
        public long storage { get; set; }
        public int views { get; set; }
        public int downloads { get; set; }
        public List<object> visitors { get; set; }
    }

    public class JSON_List
    {
        public List<JSON_FolderMetadata> folders { get; set; }
        public JSON_ListVideos videos { get; set; }
        public bool HasMore
        {
            get
            {
                return videos.next_page == null ? false : true;
            }
        }
    }
    public class JSON_ListVideos
    {
        public List<JSON_VideoMetadata> data { get; set; }
        // Public Property current_page As Integer
        // Public Property first_page As Integer
        public object next_page { get; set; }
    }

    public class JSON_VideoMetadata
    {
        public string id { get; set; }
        public object folder_id { get; set; }
        public string name { get; set; }
        public string size { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string duration { get; set; }
        public string thumbnail { get; set; }
        public bool monetize { get; set; }
        public bool @public { get; set; }
        public object error_code { get; set; }
        public string amount { get; set; }
        public string views { get; set; }
        public string downloads { get; set; }
        public int converting { get; set; }
        public int dmca { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public bool owner { get; set; }
        public JSON_MimeType mime_type { get; set; }
        public List<object> subtitles { get; set; }
        public string VideoUrl
        {
            get
            {
                return string.Concat("https://saruch.co/videos/", id);
            }
        }
    }
    public class JSON_MimeType
    {
        public string name { get; set; }
        public string type { get; set; }
        public string extension { get; set; }
    }

    public class JSON_GetFolderMetadata
    {
        public JSON_FolderMetadata folder { get; set; }
    }

    public class JSON_FolderMetadata
    {
        public string id { get; set; }
        public string name { get; set; }
        public object parent_id { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
    }

    public class JSON_GetUploadUrl
    {
        public JSON_GetUploadUrlVideo video { get; set; }
        public string server { get; set; }
    }
    public class JSON_GetUploadUrlVideo
    {
        public string id { get; set; }
        public object folder_id { get; set; }
        public string name { get; set; }
        public string size { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public List<object> subtitles { get; set; }
    }
    public class JSON_Upload
    {
        public string video_id { get; set; }
        public int finished { get; set; }
        public object expires_at { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public int uploaded { get; set; }
    }

    public class JSON_RemoteUpload
    {
        public string message { get; set; }
        [JsonProperty("invalid_urls")]public List<string> NotAcceptedUrls { get; set; }
        public List<JSON_RemoteUploadMeta> remote_uploads { get; set; }
        public bool IsErrorExists
        {
            get
            {
                return !NotAcceptedUrls.Any() ? false : true;
            }
        }
    }
    public class JSON_RemoteUploadMeta
    {
        [JsonProperty("id")]public long JobID { get; set; }
        public object video_id { get; set; }
        public string url { get; set; }
        public bool status { get; set; }
        public bool done { get; set; }
        public bool failed { get; set; }
        public string created_at { get; set; }
    }


    public class JSON_RemoteUploadQueue
    {
        public JSON_JSON_RemoteUploadQueue_Uploads remote_uploads { get; set; }
        public bool HasMore
        {
            get
            {
                return remote_uploads.next_page == null ? false : true;
            }
        }
    }
    public class JSON_JSON_RemoteUploadQueue_Uploads
    {
        public List<JSON_RemoteUploadMetadata> data { get; set; }
        // Public Property current_page As Integer
        // Public Property first_page As Integer
        public object next_page { get; set; }
    }
    public class JSON_RemoteUploadMetadata
    {
        [JsonProperty("id")]public long JobID { get; set; }
        public string video_id { get; set; }
        public string url { get; set; }
        [JsonProperty("status")]public string Progress { get; set; }
        [Browsable(false)][Bindable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)][EditorBrowsable(EditorBrowsableState.Never)]public bool done { get; set; }
        [Browsable(false)][Bindable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)][EditorBrowsable(EditorBrowsableState.Never)]public bool failed { get; set; }
        public string created_at { get; set; }
        [Browsable(false)][Bindable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)][EditorBrowsable(EditorBrowsableState.Never)]public JSON_JSON_RemoteUploadQueueVideo video { get; set; }
       public  enum RemoteUploadStatusEnum
        {
            Done,
            Faild,
            Inprogress
        }
        public RemoteUploadStatusEnum UploadStatus
        {
            get
            {
                RemoteUploadStatusEnum tobereturn= RemoteUploadStatusEnum.Inprogress;
                if (done){ tobereturn= RemoteUploadStatusEnum.Done;}
                if (done == false && failed == false) { tobereturn= RemoteUploadStatusEnum.Inprogress; }
                if (failed) { tobereturn= RemoteUploadStatusEnum.Faild; }
                return tobereturn;
            }
        }
        public JSON_JSON_RemoteUploadQueueVideo VideoMetadate
        {
            get
            {
                return video == null ? new JSON_JSON_RemoteUploadQueueVideo() : video;
            }
        }
        public string VideoUrl
        {
            get
            {
                return !string.IsNullOrEmpty(video_id) ? string.Concat("https://saruch.co/videos/", video_id) : null;
            }
        }
    }
    public class JSON_JSON_RemoteUploadQueueVideo
    {
        public string name { get; set; }
        public object mime_type { get; set; }
        public object subtitles { get; set; }
    }

    public class JSON_UploadRemoteStatus
    {
        public JSON_RemoteUploadMetadata remote_upload { get; set; }
    }

    public class JSON_RenameFolder
    {
        public Newtonsoft.Json.Linq.JToken JSON { get; set; }
        public int status { get; set; }
        [JsonProperty("result")]public bool Success { get; set; }
    }

    public class JSON_SetVideoThumbnail
    {
        public string message { get; set; }
        [JsonProperty("thumbnail")]public string ThumbnailUrl { get; set; }
    }

    public class JSON_GetSplash
    {
        public bool Success { get; set; }
        public Newtonsoft.Json.Linq.JToken JSON { get; set; }
        public int status { get; set; }
        [JsonProperty("result")]public string SplashUrl { get; set; }
    }

    public class JSON_CopyFile : Response<JSON_CopyFileResult>
    {
    }
    public class JSON_CopyFileResult
    {
        public string id { get; set; }
        public int status { get; set; }
        public string name { get; set; }
        public string size { get; set; }
        public string sha1 { get; set; }
        public string content_type { get; set; }
        public string upload_at { get; set; }
        public string cstatus { get; set; }
    }

    public class JSON_GetDownloadUrl
    {
        public JSON_GetDownloadUrlVideo video { get; set; }
        [Browsable(false)][Bindable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)][EditorBrowsable(EditorBrowsableState.Never)]public string de { get; set; }
        [Browsable(false)][Bindable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)][EditorBrowsable(EditorBrowsableState.Never)]public string en { get; set; }
    }
    public class JSON_GetDownloadUrlVideo
    {
        public string name { get; set; }
        public string size { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string duration { get; set; }
        public string thumbnail { get; set; }
        [JsonProperty("sources")]public List<JSON_GetDownloadUrlVideoSource> Downloads { get; set; }
    }
    public class JSON_GetDownloadUrlVideoSource
    {
        [Browsable(false)][Bindable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)][EditorBrowsable(EditorBrowsableState.Never)]public string file { get; set; }
        [JsonProperty("label")]public Utilitiez.ResolutionEnum Resolution { get; set; }
        [JsonProperty("type")]public string Extension { get; set; }
        public string DownloadUrl { get; set; }
    }

    public class JSON_Search
    {
        public Videos videos { get; set; }
        public bool HasMore
        {
            get
            {
                return videos.next_page == null ? false : true;
            }
        }
    }
    public class Videos
    {
        [JsonProperty("data")]public List<JSON_VideoMetadata> SearchResults { get; set; }
        public object next_page { get; set; }
    }

    public class JSON_GetConversionSetings
    {
        [Browsable(false)][Bindable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)][EditorBrowsable(EditorBrowsableState.Never)]public List<int> resolutions { get; set; }
        public bool x360
        {
            get
            {
                return resolutions.Contains(360);
            }
        }
        public bool x480
        {
            get
            {
                return resolutions.Contains(480);
            }
        }
        public bool x720
        {
            get
            {
                return resolutions.Contains(720);
            }
        }
        public bool x1080
        {
            get
            {
                return resolutions.Contains(1080);
            }
        }
    }
}


