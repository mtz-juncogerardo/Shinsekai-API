using System.Collections.Generic;
using Shinsekai_API.Models;

namespace Shinsekai_API.Requests
{
    public class ArticleRequest : ArticleItem
    {
        public List<string> ImagePaths { get; set; }
    }
}