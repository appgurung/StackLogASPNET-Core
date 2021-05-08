using System.Threading.Tasks;
using StackLog.Configuration;

namespace StackLog
{
    
    public class ILoggerBaseExtension : ILoggerService
    {
        private ILoggerService _service;
        public ILoggerBaseExtension(ILoggerService service)
        {
            _service = service;
        }
        public async Task Log(StackLogRequest request)
        {
            await _service.Log(request);
        }

        public async Task LogCloudWatch(StackLogResponse main)
        {
            await _service.LogCloudWatch(main);
        }
    }
}