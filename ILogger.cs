using System.Threading.Tasks;
using Newtonsoft.Json;
using Refit;
using StackLog.Backend;
using StackLog.Configuration;

namespace StackLog
{
    public interface ILoggerService
    {
        
        Task Log(StackLogRequest request);
        Task LogCloudWatch(StackLogExceptionInformation main);
    }

    public sealed class LoggerService : ILoggerService
    {
        private string stackKey;
        private IStackLogHost host;

        private string bucketKey;

        private string secretKey;

        private IStackLogHostResponse _response;

        private bool enableCloudWatch;
        //private IStackLog stakcLog;
        public LoggerService(string bucketKey, string secretKey, bool enableCloudWatch=false)
        {
            stackKey = "cToErsOcQ";
            host = RestService.For<IStackLogHost>("http://www.stacklog.io:8540/");
            this.bucketKey = bucketKey;
            this.secretKey = secretKey;
            this.enableCloudWatch = enableCloudWatch;
        }
        public async Task Log(StackLogRequest request)
        {
            await Task.Factory.StartNew(async () =>
            {
                ApiResponse<IStackLogHostResponse> responseFromClientCall =
                    await host.CreateLogs(bucketKey, this.secretKey, request);
                _response = responseFromClientCall.Content;
                
                if (_response != null)
                {
                    if(_response.responseCode != "00")
                    {
                        throw new StackLogException(JsonConvert.SerializeObject(_response.responseData));
                    }
                    
                }
               
                throw new StackLogException(JsonConvert.SerializeObject(new { message = "unable to create log, " +
                        "please pass appropriate secret key and bucket key"}));
                
            });

        }

        public async Task LogCloudWatch(StackLogExceptionInformation main)
        {
            if (this.enableCloudWatch)
            {
                await Task.Factory.StartNew(async () =>
                {
                    ApiResponse<IStackLogHostResponse>  responseFromClientCall = await host.LogCloudWatch(this.bucketKey, this.secretKey, this.stackKey, main);
                    _response = responseFromClientCall.Content;

                    if (_response != null)
                    {
                        if (_response.responseCode != "00")
                        {
                            throw new StackLogException(JsonConvert.SerializeObject(_response.responseData));
                        }

                    }
                   
                    throw new StackLogException(JsonConvert.SerializeObject(new { message = "unable to create log, " +
                            "please pass appropriate secret key and bucket key" }));
                });
            }
            
        }
    }
}