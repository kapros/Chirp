using Chirp.Contracts.V1.Events;
using Chirp.Data;
using Chirp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostsEventProcessor
{
    public class PostService : IPostService
    {
        private readonly DataContext _context;
        private readonly ITagService _tagService;

        public PostService(DataContext context, ITagService tagService)
        {
            _context = context;
            _tagService = tagService;
        }

        public async Task<Post> Create(CreatePostDto post)
        {
            await _context.Posts.AddAsync(post.Post);
            var tags = await _tagService.GetTagsByNames(post.Tags);
            post.Post.Tags.AddRange(tags.Select(x => new PostTag { PostId = post.Post.Id, Tag = x }));
            await _context.SaveChangesAsync();
            return post.Post;
        }
    }
}
