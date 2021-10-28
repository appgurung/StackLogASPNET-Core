using System;
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

        public async Task LogFatal(string message)
        {
            await _logger.LogFatal(message);
        }

        public async Task LogFatal(Exception es)
        {
            await _logger.LogFatal(es);
        }

        public async Task LogInformation(string message)
        {
            await _logger.Info(message);
        }

        public async Task LogDebug(string message)
        {
            await _logger.LogDebug(message);
        }

        public async Task LogWarning(string message)
        {
            await _logger.LogWarning(message);
        }

        public async Task LogCloudWatch(StackLogResponse logInformation)
        {
            await _logger.LogCloudWatch(logInformation);
        }

        public async Task LogError(string message)
        {
            await _logger.LogWarning(message);
        }

        Task<IStackLog> IStackLog.LogFatal(string message)
        {
            throw new NotImplementedException();
        }

        Task<IStackLog> IStackLog.LogFatal(Exception es)
        {
            throw new NotImplementedException();
        }

        public Task<IStackLog> Info(string message)
        {
            throw new NotImplementedException();
        }

        Task<IStackLog> IStackLog.LogDebug(string message)
        {
            throw new NotImplementedException();
        }

        Task<IStackLog> IStackLog.LogWarning(string message)
        {
            throw new NotImplementedException();
        }

        Task<IStackLog> IStackLog.LogCloudWatch(StackLogResponse logInformation)
        {
            throw new NotImplementedException();
        }

        Task<IStackLog> IStackLog.LogError(string message)
        {
            throw new NotImplementedException();
        }

        public Task To(string buckey)
        {
            throw new NotImplementedException();
        }

        public Task To(params string[] bucketKeys)
        {
            throw new NotImplementedException();
        }
    }
}