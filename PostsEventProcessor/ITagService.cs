using Chirp.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PostsEventProcessor
{
    public interface ITagService
    {
        Task<Tag> GetTagByName(string name);

        Task<IEnumerable<Tag>> GetTagsByNames(IEnumerable<string> names);

        Task<IEnumerable<Tag>> GetAllTags();
    }
}
