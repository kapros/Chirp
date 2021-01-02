using Chirp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public interface IPostService
    {
        Task<IList<Post>> GetPostsAsync(GetAllPostsFilter filters = null, PaginationFilter paginationFilter = null);

        Task<Post> GetPostByIdAsync(Guid id);

        Task<bool> UpdatePostAsync(Post postToupdate);

        Task<bool> DeletePostAsync(Guid id);

        Task<bool> CreatePostAsync(Post post);
        Task<(bool Success, bool PostFound, bool UserOwnsPost)> UserOwnsPost(Guid id, string v);
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag> GetTagByNameAsync(string name);
        Task<bool> CreateTagAsync(Tag newTag);
        Task<bool> DeleteTagAsync(string tagName);
        Task<bool> UpdateTagAsync(Tag tag);
    }
}
