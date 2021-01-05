using Chirp.Data;
using Chirp.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _context;

        public PostService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            var created = await _context.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeletePostAsync(Guid id)
        {
            var post = await GetPostByIdAsync(id);

            if (post == null)
                return false;

            _context.Posts.Remove(post);
            var deleted = await _context.SaveChangesAsync();
            return deleted > 0;
        }
        public async Task<bool> UpdatePostAsync(Post postToupdate)
        {
            _context.Posts.Update(postToupdate);
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<Post> GetPostByIdAsync(Guid id)
        {
            return await _context.Posts.Include(x => x.Tags).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<Post>> GetPostsAsync(GetAllPostsFilter filter = null, PaginationFilter paginationFilter = null)
        {
            var queryable = _context.Posts.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable.Include(x => x.Tags).ToListAsync();
            }

            queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable.Include(x => x.Tags)
                .Skip(skip)
                .Take(paginationFilter.PageSize)
                .ToListAsync();
        }
        public async Task<(bool Success, bool PostFound, bool UserOwnsPost)> UserOwnsPost(Guid postId, string userId)
        {
            var post = await _context.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);
            var postFound = post != null;
            var userOwnsPost = post?.UserId == userId;
            var success = postFound && userOwnsPost;
            return (success, postFound, userOwnsPost);
        }

        private static IQueryable<Post> AddFiltersOnQuery(GetAllPostsFilter filter, IQueryable<Post> queryable)
        {
            if (string.IsNullOrWhiteSpace(filter?.UserId))
            {
                queryable = queryable.Where(x => x.UserId == filter.UserId);
            }

            return queryable;
        }
    }
}
