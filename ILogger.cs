using System;
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
        Task LogCloudWatch(StackLogResponse main);
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
        public LoggerService(StackLogOptions options=null)
        {
            if(options != null)
            {
                if(!options.enableFileLogging ||  !options.enableConsoleLogging)
                {
                    if (String.IsNullOrEmpty(options.bucketKey) || String.IsNullOrEmpty(options.secretKey))
                    {
                        throw new StackLogException(StackLogExceptionErrors.BUCKET_KEY_MISSING + "and or " + StackLogExceptionErrors.SECRET_KEY_MISSING);
                    }
                }
            }
           
            if(options == null)
            {
                throw new StackLogException(StackLogExceptionErrors.NULL_OPTIONS);
            }
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
                        throw new StackLogException(StackLogExceptionErrors.MakeError(JsonConvert.SerializeObject(_response.responseData)));
                    }
                    
                }
               
                throw new StackLogException(StackLogExceptionErrors.UNABLE_TO_CREATE_LOG);
                
            });

        }

        public async Task LogCloudWatch(StackLogResponse main)
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
                            throw new StackLogException(StackLogExceptionErrors.MakeError(JsonConvert.SerializeObject(_response.responseData)));
                        }

                    }
                   
                    throw new StackLogException(StackLogExceptionErrors.UNABLE_TO_CREATE_LOG);
                });
            }
            
        }
    }
}