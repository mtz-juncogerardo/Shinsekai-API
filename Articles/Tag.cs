namespace Shinsekai_API.Responses
{
    public class Tag: ITag
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Tag()
        {
        }
        
        public Tag(ITag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
        }
    }

    public interface ITag
    {
        public string Id { get; set; }
        public string Name { get; set; } 
    }
}