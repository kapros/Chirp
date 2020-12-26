using Chirp.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public interface IUriService
    {
        Uri GetPostUri(Guid postId);

        Uri GetAllPostsUri(PaginationQuery paginationQuery = null);
    }
}
