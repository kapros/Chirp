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

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            var created = await _context.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeletePostAsync(Guid id)
        {
            var post = await GetPostbyIdAsync(id);

            if (post == null)
                return false;

            _context.Posts.Remove(post);
            var deleted = await _context.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> DeleteTagAsync(string name)
        {
            var tag = await GetTagByNameAsync(name);

            if (tag == null)
                return false;

            _context.Tags.Remove(tag);
            var deleted = await _context.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            var tags = await _context.Tags.ToListAsync();
            return tags;
        }

        public async Task<Post> GetPostbyIdAsync(Guid id)
        {
            return await _context.Posts.SingleOrDefaultAsync(x => x.Id == id);
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

        public async Task<Tag> GetTagByNameAsync(string name)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Name == name);
            return tag;
        }

        public async Task<bool> UpdatePostAsync(Post postToupdate)
        {
            _context.Posts.Update(postToupdate);
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> UpdateTagAsync(Tag tagToUpdate)
        {
            _context.Tags.Update(tagToUpdate);
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> UserOwnsPost(Guid postId, string userId)
        {
            var post = await _context.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);

            return post?.UserId == userId;
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
