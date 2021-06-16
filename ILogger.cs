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
        string baseUrl = "http://www.stacklog.io:8873/";
        //private IStackLog stakcLog;
        public LoggerService(IStackLogOptions options)
        {
            if(options != null)
            {
                if(options.OnPremiseOptions != null)
                {
                   // if()
                    //if(options.OnPremiseOptions.enable)
                    //{
                        if(String.IsNullOrEmpty(options.OnPremiseOptions.baseUrl))
                        {
                            throw new StackLogException(StackLogExceptionErrors.OnPrem_BaseUrl_Not_Found);
                        }

                        baseUrl = options.OnPremiseOptions.baseUrl;
                    //}
                }
                if(options.FileOptions != null)
                {
                    if(!options.FileOptions.enable)
                    {
                        if (String.IsNullOrEmpty(options.bucketKey) || String.IsNullOrEmpty(options.secretKey))
                        {
                            throw new StackLogException(StackLogExceptionErrors.BUCKET_KEY_MISSING + "and or " + StackLogExceptionErrors.SECRET_KEY_MISSING);
                        }
                    }
                    
                }
            }
           
            if(options == null)
            {
                throw new StackLogException(StackLogExceptionErrors.NULL_OPTIONS);
            }
            stackKey = "cToErsOcQ";
            host = RestService.For<IStackLogHost>(baseUrl);
            this.bucketKey = options.bucketKey;
            this.secretKey = options.secretKey;
            this.enableCloudWatch = options.enableCloudWatch;
        }
        public async Task Log(StackLogRequest request)
        {
            request.stackKey = this.stackKey;
            //await Task.Factory.StartNew(async () =>
            //{
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
         
                if(_response == null)
                {
                    throw new StackLogException(StackLogExceptionErrors.UNABLE_TO_CREATE_LOG);
                }
                

            // });
          //  return Task.CompletedTask;

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

                    //           throw new StackLogException(StackLogExceptionErrors.UNABLE_TO_CREATE_LOG);
                    if (_response == null)
                    {
                        throw new StackLogException(StackLogExceptionErrors.UNABLE_TO_CREATE_LOG);
                    }
                });
            }
            
        }
    }
}