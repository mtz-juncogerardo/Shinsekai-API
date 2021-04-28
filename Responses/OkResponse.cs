namespace Shinsekai_API.Responses
{
    public class OkResponse : IApiResponse
    {
        public object Response { get; set; }
        public int Count { get; set; }
        public int Page { get; set; }
        public int MaxPage { get; set; }
    }
}