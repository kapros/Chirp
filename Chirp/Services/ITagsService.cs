using Chirp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public interface ITagsService
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag> GetTagByNameAsync(string name);
        Task<bool> CreateTagAsync(Tag newTag);
        Task<bool> DeleteTagAsync(string tagName);
        Task<bool> UpdateTagAsync(Tag tag);
    }
}
