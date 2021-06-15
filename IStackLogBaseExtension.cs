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
            _logger.LogFatal(message);
        }

        public async Task LogFatal(Exception es)
        {
            _logger.LogFatal(es);
        }

        public async Task LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public async Task LogDebug(string message)
        {
            _logger.LogDebug(message);
        }

        public async Task LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public async Task LogCloudWatch(StackLogResponse logInformation)
        {
            _logger.LogCloudWatch(logInformation);
        }

        public async Task LogError(string message)
        {
            _logger.LogWarning(message);
        }
    }
}