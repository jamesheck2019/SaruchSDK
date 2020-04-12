using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaruchSDK.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static SaruchSDK.Basic;
using static SaruchSDK.Utilitiez;

namespace SaruchSDK
{
    public class FolderClient : IFolder
    {
        private string FolderID { get; set; }
        public FolderClient(string FolderID) => this.FolderID = FolderID;


        public async Task<JSON_List> List(int OffSet = 1, SortEnum Sort = SortEnum.name, OrderByEnum OrderBy = OrderByEnum.asc)
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Get, Build("video-manager", new Dictionary<string, string>() { { "per_page", "50" }, { "folder_id", FolderID }, { "page", OffSet.ToString() }, { "sort", JsonConvert.SerializeObject(new { where = Sort.ToString(), by = OrderBy.ToString() }) } }));
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_List>(result, JSONhandler) : throw ShowError(result);
        }

        public async Task<JSON_FolderMetadata> Metadata()
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Get, new pUri($"folders/{FolderID}").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_FolderMetadata>(JObject.Parse(result).SelectToken("folder").ToString(), JSONhandler) : throw ShowError(result);
        }

        public async Task<JSON_FolderMetadata> Create(string FolderName)
        {
            var parameters = new Dictionary<string, string>() { { "parent_id", FolderID }, { "name", FolderName } };

            HttpResponseMessage response = await RequestAsync(HttpMethod.Post, new pUri("folders").ToString(), parameters.EncodedContent());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_FolderMetadata>(result, JSONhandler) : throw ShowError(result);
        }

        public async Task<bool> Rename(string NewName)
        {
            var parameters = new Dictionary<string, string>() { { "name", NewName } };

            HttpResponseMessage response = await RequestAsync(HttpMethod.Put, new pUri($"folders/{FolderID}").ToString(), parameters.EncodedContent());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? true : throw ShowError(result);
        }

        public async Task<bool> Move(string DestinationFolderID)
        {
            var parameters = new Dictionary<string, string>() { { "folder_id", DestinationFolderID } };
            HttpResponseMessage response = await RequestAsync(HttpMethod.Put, new pUri($"folders/{FolderID}").ToString(), parameters.EncodedContent());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? true : throw ShowError(result);
        }

        public async Task<bool> Delete()
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Delete, new pUri($"folders/{FolderID}").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? true : throw ShowError(result);
        }

        private async Task<JSON_GetUploadUrl> GetUploadUrl(string FileName, string FileSize)
        {
            if (Path.GetExtension(FileName).ToLower() == ".wmv"){throw new SaruchException ("Mime type not allowed",405);}

            var parameters = new Dictionary<string, string>
            {
                { "name", FileName },
                { "size", FileSize },
                { "mime_type", "video/mp4" }, // Web.MimeMapping.GetMimeMapping(FileName))
                { "folder_id", FolderID }
            };

            HttpResponseMessage response = await RequestAsync(HttpMethod.Post, new pUri("videos").ToString(), parameters.EncodedContent());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_GetUploadUrl>(result, JSONhandler) : throw ShowError(result);
        }

        public async Task<JSON_Upload> Upload(object FileToUpload, UploadTypes UploadType, string FileName, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default)
        {
            long filesize = 0;
            ReportCls = ReportCls ?? new Progress<ReportStatus>();
            ReportCls.Report(new ReportStatus() { Finished = false, TextStatus = "Initializing..." });
            try
            {
                System.Net.Http.Handlers.ProgressMessageHandler progressHandler = new System.Net.Http.Handlers.ProgressMessageHandler(new HCHandler());
                progressHandler.HttpSendProgress += (sender, e) => { ReportCls.Report(new ReportStatus() { ProgressPercentage = e.ProgressPercentage, BytesTransferred = e.BytesTransferred, TotalBytes = e.TotalBytes ?? 0, TextStatus = "Uploading..." }); };
                using (HtpClient localHttpClient = new HtpClient(progressHandler))
                {
                    HttpRequestMessage HtpReqMessage = new HttpRequestMessage();
                    HtpReqMessage.Method = HttpMethod.Post;
                    var MultipartsformData = new MultipartFormDataContent();
                    HttpContent streamContent = null;
                    switch (UploadType)
                    {
                        case UploadTypes.FilePath:
                            streamContent = new StreamContent(new FileStream(FileToUpload.ToString(), FileMode.Open, FileAccess.Read));
                            filesize = new FileInfo(FileToUpload.ToString()).Length;
                            break;
                        case UploadTypes.Stream:
                            var strm = (Stream)FileToUpload;
                            streamContent = new StreamContent(strm);
                            filesize = strm.Length;
                            break;
                        case UploadTypes.BytesArry:
                            byte[] byt = (byte[])FileToUpload;
                            streamContent = new StreamContent(new MemoryStream(byt));
                            filesize = byt.Length;
                            break;
                    }
                    streamContent.Headers.Clear();
                    streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "\"video\"", FileName = "\"" + FileName + "\"" };
                    // '''''''''''''''''''''''''
                    var uploadUrl = await GetUploadUrl(FileName, filesize.ToString());
                    MultipartsformData.Add(new StringContent(uploadUrl.video.id), "\"video_id\"");
                    MultipartsformData.Add(new StringContent("resumeable"), "\"upload_type\"");
                    MultipartsformData.Add(streamContent);
                    HtpReqMessage.Content = MultipartsformData;
                    HtpReqMessage.RequestUri = new Uri(uploadUrl.server);
                    // ''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
                    using (HttpResponseMessage ResPonse = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false))
                    {
                        string result = await ResPonse.Content.ReadAsStringAsync();

                        token.ThrowIfCancellationRequested();
                        if (ResPonse.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = $"[{FileName}] Uploaded successfully" });
                            return JsonConvert.DeserializeObject<JSON_Upload>(result, JSONhandler);
                        }
                        else
                        {
                            ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = $"The request returned with HTTP status code {ResPonse.ReasonPhrase}" });
                            throw ShowError(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReportCls.Report(new ReportStatus() { Finished = true });
                if (ex.Message.ToString().ToLower().Contains("a task was canceled"))
                {
                    ReportCls.Report(new ReportStatus() { TextStatus = ex.Message });
                }
                else
                {
                    throw new SaruchException (ex.Message, 1001);
                }
                return null;
            }
        }

        public async Task<JSON_RemoteUpload> UploadRemotly(List<string> FileUrls)
        {
            var JSONobj = new { urls = string.Join(@"\", FileUrls) };
            HttpContent streamContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(JSONobj), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await RequestAsync(HttpMethod.Post, new pUri("remote-uploads").ToString(), streamContent);
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_RemoteUpload>(result, JSONhandler) : throw ShowError(result);
        }

    }
}
