using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaruchSDK.JSON;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static SaruchSDK.Basic;
using static SaruchSDK.Utilitiez;


namespace SaruchSDK
{
    public class SubtitlesClient : ISubtitles
    {
        private string VideoID { get; set; }
        public SubtitlesClient(string VideoID) => this.VideoID = VideoID;


        public async Task<JSON_Upload> Upload(object FileToUpload, UploadTypes UploadType, LanguageCodeEnum LanguageCode, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default)
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
                    HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new Uri($"https//api.saruch.co/videos/{VideoID}/subtitles"));
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
                            var byt = (byte[])FileToUpload;
                            streamContent = new StreamContent(new MemoryStream(byt));
                            filesize = byt.Length;
                            break;
                    }
                    streamContent.Headers.Clear();
                    streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "\"subtitle\"" };
                    // '''''''''''''''''''''''''
                    MultipartsformData.Add(new StringContent(stringValueOf(LanguageCode)), "\"language_code\"");
                    MultipartsformData.Add(new StringContent("true"), "\"force\"");
                    MultipartsformData.Add(streamContent);
                    HtpReqMessage.Content = MultipartsformData;
                    // ''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
                    using (HttpResponseMessage ResPonse = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false))
                    {
                        string result = await ResPonse.Content.ReadAsStringAsync();

                        token.ThrowIfCancellationRequested();
                        if (ResPonse.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = "Subtitle Uploaded successfully" });
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
                    throw new SaruchException(ex.Message, 1001);
                }
                return null;
            }
        }

        public async Task<JSON_FolderMetadata> Metadata()
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Get, new pUri($"videos/{VideoID}/subtitles").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_FolderMetadata>(JObject.Parse(result).SelectToken("folder").ToString(), JSONhandler) : throw ShowError(result);
        }

        public async Task<JSON_FolderMetadata> Metadata2(string DestinationSubtitleID)
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Get, new pUri($"videos/{VideoID}/subtitles/{DestinationSubtitleID}").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_FolderMetadata>(JObject.Parse(result).SelectToken("folder").ToString(), JSONhandler) : throw ShowError(result);
        }

        public async Task<JSON_FolderMetadata> Delete(string DestinationSubtitleID)
        {
            HttpResponseMessage response = await RequestAsync(HttpMethod.Get, new pUri($"videos/{VideoID}/subtitles/{DestinationSubtitleID}").ToString());
            string result = await response.Content.ReadAsStringAsync();
            return response.Success() ? JsonConvert.DeserializeObject<JSON_FolderMetadata>(JObject.Parse(result).SelectToken("folder").ToString(), JSONhandler) : throw ShowError(result);
        }


    }
}

