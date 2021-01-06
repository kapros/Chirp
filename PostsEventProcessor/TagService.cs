using Chirp.Data;
using Chirp.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostsEventProcessor
{
    public class TagService : ITagService
    {
        private readonly DataContext _dataContext;

        public TagService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            return await _dataContext.Tags.ToListAsync();
        }

        public async Task<Tag> GetTagByName(string name)
        {
            return await _dataContext.Tags.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<Tag>> GetTagsByNames(IEnumerable<string> names)
        {
            return await _dataContext.Tags.Where(x => names.Any(name => name == x.Name)).ToListAsync();
        }
    }
}
