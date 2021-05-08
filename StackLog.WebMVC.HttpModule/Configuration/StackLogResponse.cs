namespace StackLog.WebMVC.HttpModule.Configuration
{
    public class StackLogResponse
    {
        public string executedSqlQueries { get; set; }
        public int statusCode { get; set; }
        public bool isRequest { get; set; }
        public string httpResponse { get; set; }
        public string controllerMethod { get; set; }
        public string controllerName { get; set; }
        public string methodAttributes { get; set; }
        public string requestParam { get; set; }
        public object header { get; set; }
        public string url { get; set; }
        public string httpVerb { get; set; }
        public string httpProtocol { get; set; }
        public string userAgent { get; set; }
        public string remoteIpAddress { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string queryString { get; set; }
        public string requestId { get; set; }
    }
}