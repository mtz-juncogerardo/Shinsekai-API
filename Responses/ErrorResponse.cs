namespace Shinsekai_API.Responses
{
    public class ErrorResponse : IApiResponse
    {
        public object Error { get; set; }
        public int Count { get; set; }
    }
}