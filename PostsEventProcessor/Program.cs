using Chirp.Contracts;
using Chirp.Contracts.V1.Events;
using Chirp.Data;
using Chirp.Domain;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace PostsEventProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        private static CreatePostDto MapPost(NewPostEventDto eventDto)
        {
            return new CreatePostDto
            {
                Post = eventDto.Adapt<Post>(),
                Tags = eventDto.Tags
            };
        }

        private static async Task CreatePost(Event<NewPostEventDto> eventDto, IPostService postService, AcceptedJobsContext acceptedJobsContext)
        {
            var post = await postService.Create(MapPost(eventDto.Message));
            var job = await acceptedJobsContext.Jobs.FirstOrDefaultAsync(x => x.JobId == eventDto.EventId);
            job.DateFinished = DateTime.UtcNow;
            job.CreatedObjectId = post.Id;
            await acceptedJobsContext.SaveChangesAsync();
        }
    }
}
