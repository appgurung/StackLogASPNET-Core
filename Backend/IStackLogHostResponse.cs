namespace StackLog.Backend
{
    public class IStackLogHostResponse
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public object responseData { get; set; }
    }
}