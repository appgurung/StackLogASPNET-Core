using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using StackLog.Configuration;

namespace StackLog
{
    

    public class StackLogBaseExtension : IStackLog
    {
        private IStackLog _logger;

      

        public StackLogBaseExtension(IStackLog logger)
        {
            _logger = logger;
        }

        public string bucketKey => _logger.bucketKey;

        public string secretKey => _logger.secretKey;

        public async Task<IStackLog> LogFatal(string message, [Optional] string x)
        {
            await _logger.LogFatal(message);
            return _logger;
        }

        public async Task<IStackLog> LogFatal(Exception es,  [Optional] string x)
        {
            await _logger.LogFatal(es);

            return _logger;
        }

        public async Task<IStackLog> Info(string message, [Optional] string x)
        {
            await _logger.Info(message, null);
            return _logger;
        }

        public async Task<IStackLog> LogDebug(string message, [Optional] string x)
        {
            await _logger.LogDebug(message);
            return _logger;
        }

        public async Task<IStackLog> LogWarning(string message, [Optional] string x)
        {
            await _logger.LogWarning(message);
            return _logger;
        }

        public async Task<IStackLog> LogCloudWatch(StackLogResponse logInformation, [Optional] string x)
        {
            await _logger.LogCloudWatch(logInformation);
            return _logger;
        }

        public async Task<IStackLog> LogError(string message, [Optional] string y)
        {
            await _logger.LogWarning(message);
            return this;
        }

        public StackLogExtension Info(string message)
        {
            throw new NotImplementedException();
        }

        public StackLogExtension Debug(string message)
        {
            throw new NotImplementedException();
        }

        public StackLogExtension Warning(string message)
        {
            throw new NotImplementedException();
        }

        public StackLogExtension CloudWatch(StackLogResponse logInformation)
        {
            throw new NotImplementedException();
        }
    }
}