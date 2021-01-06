using Chirp.Domain;
using System.Collections.Generic;

namespace PostsEventProcessor
{
    public class CreatePostDto
    {
        public Post Post { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}