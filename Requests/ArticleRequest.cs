namespace Shinsekai_API.Requests
{
    public class ArticleRequest
    {
        public string Id { get; set; }
        public bool ByAnime { get; set; }
        public bool ByLine { get; set; }
        public bool ByMaterial { get; set; }
    }
}