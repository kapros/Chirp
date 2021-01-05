using Chirp.Data;
using Chirp.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public class TagsService : ITagsService
    {
        private readonly DataContext _context;

        public TagsService(DataContext context)
        {
            _context = context;
        }

        public async Task<Tag> GetTagByNameAsync(string name)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Name == name);
            return tag;
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            var created = await _context.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UpdateTagAsync(Tag tagToUpdate)
        {
            _context.Tags.Update(tagToUpdate);
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
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
    }
}