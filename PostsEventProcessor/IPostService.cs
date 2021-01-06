using Chirp.Contracts.V1.Events;
using Chirp.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PostsEventProcessor
{
    public interface IPostService
    {
        Task<Post> Create(CreatePostDto post);
    }
}
