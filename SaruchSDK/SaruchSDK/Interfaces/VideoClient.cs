using Newtonsoft.Json;
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
    public class VideoClient : IVideo
    {
        private string VideoID { get; set; }
        public VideoClient(string VideoID) => this.VideoID = VideoID;

        public ISubtitles Subtitles => new SubtitlesClient(VideoID);


        public async Task<JSON_GetDownloadUrl> GetDownloadUrl()
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Post, new pUri($"videos/{VideoID}/stream").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_GetDownloadUrl>(result, JSONhandler) : throw ShowError(result);
        }

        public async Task Download(string StreamVideoUrl, string FileSavePath, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default)
        {
            ReportCls = ReportCls ?? new Progress<ReportStatus>();
            ReportCls.Report(new ReportStatus() { Finished = false, TextStatus = "Initializing..." });
            try
            {
                HCHandler hand = new HCHandler() { MaxRequestContentBufferSize = 1 * 1024 * 1024, AllowAutoRedirect = true, MaxAutomaticRedirections = 6 };
                System.Net.Http.Handlers.ProgressMessageHandler progressHandler = new System.Net.Http.Handlers.ProgressMessageHandler(hand);
                progressHandler.HttpReceiveProgress += (sender, e) => { ReportCls.Report(new ReportStatus() { ProgressPercentage = e.ProgressPercentage, BytesTransferred = e.BytesTransferred, TotalBytes = e.TotalBytes ?? 0, TextStatus = "Downloading..." }); };
                HttpClient localHttpClient = new HttpClient(progressHandler);
                localHttpClient.DefaultRequestHeaders.Referrer = new Uri($"https://saruch.co/videos/{VideoID}/");
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(StreamVideoUrl));
                // ''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
                using (HttpResponseMessage ResPonse = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false))
                {
                    if (ResPonse.IsSuccessStatusCode)
                    {
                        ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = $"[{Path.GetFileName(FileSavePath)}] Downloaded successfully." });
                    }
                    else
                    {
                        ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = $"Error code: {ResPonse.StatusCode}" });
                    }

                    ResPonse.EnsureSuccessStatusCode();
                    var stream_ = await ResPonse.Content.ReadAsStreamAsync();
                    using (var fileStream = new FileStream(FileSavePath, FileMode.Append, FileAccess.Write))
                    {
                        stream_.CopyTo(fileStream);
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
                    throw new SaruchException(ex.Message, 1001);
                }
            }
        }

        public async Task<bool> SetVideoCategory(VideoCategoryEnum Category)
        {
            var parameters = new Dictionary<string, string>() { { "video_type_id", ((int)Category).ToString() } };
            HttpResponseMessage response = await RequestAsync(HttpMethod.Put, new pUri($"videos/{VideoID}").ToString(), parameters.EncodedContent());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? true : throw ShowError(result);
        }

        public async Task<bool> Rename(string NewName)
        {
            var parameters = new Dictionary<string, string>() { { "name", NewName } };
            HttpResponseMessage response = await RequestAsync(HttpMethod.Put, new pUri($"videos/{VideoID}").ToString(), parameters.EncodedContent());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? true : throw ShowError(result);
        }

        public async Task<bool> Delete()
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Delete, new pUri("videos/{VideoID}").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? true : throw ShowError(result);
        }

        public async Task<bool> Move(string DestinationFolderID)
        {
            var parameters = new Dictionary<string, string>() { { "folder_id", DestinationFolderID } };
            HttpResponseMessage response = await RequestAsync(HttpMethod.Put, new pUri($"videos/{VideoID}").ToString(), parameters.EncodedContent());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? true : throw ShowError(result);
        }

        public async Task<bool> Privacy(PrivacyEnum SetPrivacy)
        {
            var JSONobj = new { @public = Convert.ToBoolean(SetPrivacy) };
            HttpContent streamContent = new StringContent(JsonConvert.SerializeObject(JSONobj), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await RequestAsync(HttpMethod.Put, new pUri("videos/{VideoID}").ToString(), streamContent);
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? true : throw ShowError(result);
        }

        public async Task<bool> Monetize(bool SetMonetize)
        {
            var JSONobj = new { monetize = SetMonetize };
            HttpContent streamContent = new StringContent(JsonConvert.SerializeObject(JSONobj), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await RequestAsync(HttpMethod.Put, new pUri($"videos/{VideoID}").ToString(), streamContent);
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? true : throw ShowError(result);
        }

        public async Task<JSON_SetVideoThumbnail> ThumbnailSet(object FileToUpload, UploadTypes UploadType, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default)
        {
            ReportCls = ReportCls ?? new Progress<ReportStatus>();
            ReportCls.Report(new ReportStatus() { Finished = false, TextStatus = "Initializing..." });
            try
            {
                System.Net.Http.Handlers.ProgressMessageHandler progressHandler = new System.Net.Http.Handlers.ProgressMessageHandler(new HCHandler());
                progressHandler.HttpSendProgress += (sender, e) => { ReportCls.Report(new ReportStatus() { ProgressPercentage = e.ProgressPercentage, BytesTransferred = e.BytesTransferred, TotalBytes = e.TotalBytes ?? 0, TextStatus = "Uploading..." }); };
                using (HttpClient localHttpClient = new HttpClient(progressHandler))
                {
                    HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri($"videos/{VideoID}/thumbnail"));
                    var MultipartsformData = new MultipartFormDataContent();
                    HttpContent streamContent = null;
                    switch (UploadType)
                    {
                        case UploadTypes.FilePath:
                            streamContent = new StreamContent(new FileStream(FileToUpload.ToString(), FileMode.Open, FileAccess.Read));
                            break;
                        case UploadTypes.Stream:
                            streamContent = new StreamContent((Stream)FileToUpload);
                            break;
                        case UploadTypes.BytesArry:
                            streamContent = new StreamContent(new MemoryStream((byte[])FileToUpload));
                            break;
                    }
                    streamContent.Headers.Clear();
                    streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "\"thumbnail\"", FileName = "\"blob\"" };
                    MultipartsformData.Add(streamContent);
                    HtpReqMessage.Content = MultipartsformData;
                    // ''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
                    using (HttpResponseMessage ResPonse = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false))
                    {
                        string result = await ResPonse.Content.ReadAsStringAsync();

                        token.ThrowIfCancellationRequested();
                        if (ResPonse.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = ("Thumbnail Uploaded successfully") });
                            return JsonConvert.DeserializeObject<JSON_SetVideoThumbnail>(result, JSONhandler);
                        }
                        else
                        {
                            ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = $"The request returned with HTTP status code {ResPonse.ReasonPhrase}" });
                            var errorInfo = JsonConvert.DeserializeObject<JSON_Error>(result, JSONhandler);
                            throw new SaruchException(errorInfo._ErrorMessage, (int)ResPonse.StatusCode);
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

        public async Task<bool> ThumbnailDelete()
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Delete, new pUri("videos/{VideoID}/thumbnail").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? true : throw ShowError(result);
        }


    }
}
