namespace Shinsekai_API.Responses
{
    public class Image : IImage
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public long Order { get; set; }

        public Image()
        {
        }
        
        public Image(IImage image)
        {
            Id = image.Id;
            Path = image.Path;
            Order = image.Order;
        }
    }

    public interface IImage
    {
        public string Id { get; set; }
        public string Path { get; set; } 
        public long Order { get; set; }
    }
}