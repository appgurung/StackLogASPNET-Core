using System.Threading.Tasks;
using Refit;
using StackLog.Configuration;

namespace StackLog.Backend
{
    public interface IStackLogHost
    {
        [Post("/api/v1/sandbox/log/create/session/{bucketKey}")]
        Task <ApiResponse<IStackLogHostResponse>> LogCloudWatch(string bucketKey, [Header("secretKey")] string secretKey, [Header("stackKey")] string stackKey, [Body] StackLogResponse action);

        // [Post("/api/v1/sandbox/log/create/{bucketKey}")]
        // Task<ApiResponse<IStackLogHostResponse>> LogFatal(string bucketKey,[Header("secretKey")] string secretKey, [Body]StackLogRequest request);
        //
        // [Post("/api/v1/sandbox/log/create/{bucketKey}")]
        // Task<ApiResponse<IStackLogHostResponse>> LogDebug(string bucketKey, [Header("secretKey")] string secretKey, [Body]StackLogRequest request);
        //
        // [Post("/api/v1/sandbox/log/create/{bucketKey}")]
        // Task<ApiResponse<IStackLogHostResponse>> LogError(string bucketKey, [Header("secretKey")] string secretKey, [Body]StackLogRequest request);
        //
        // [Post("/api/v1/sandbox/log/create/{bucketKey}")]
        // Task<ApiResponse<IStackLogHostResponse>> LogWarning(string bucketKey, [Header("secretKey")] string secretKey, [Body]StackLogRequest request);

        [Post("/api/v1/sandbox/log/create/{bucketKey}")]
        Task<ApiResponse<IStackLogHostResponse>> CreateLogs(string bucketKey, [Header("secretKey")] string secretKey, [Body]StackLogRequest request);
    }
}