using Chirp.Domain;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public class CosmosDbPostService : IPostService
    {
        private readonly CosmosClient _client;
        private const string DATABASE = "PostsDatabase";
        private const string CONTAINERID = "Posts";

        public CosmosDbPostService(CosmosClient client)
        {
            _client = client;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            var db = _client.GetDatabase(DATABASE);
            var container = db.GetContainer(CONTAINERID);
            var result = await container.CreateItemAsync(post);
            var statusCode = result.StatusCode;
            return true;
        }

        public async Task<bool> DeletePostAsync(Guid id)
        {
            var db = _client.GetDatabase(DATABASE);
            var container = db.GetContainer(CONTAINERID);
            try
            {
                var deleted = await container.DeleteItemAsync<Post>(id.ToString(), new PartitionKey(id.ToString()));
            }
            catch (CosmosException cex)
            {
                // log error here
                return false;
            }
            return true;
        }

        public async Task<Post> GetPostbyIdAsync(Guid id)
        {
            var db = _client.GetDatabase(DATABASE);
            var container = db.GetContainer(CONTAINERID);
            var queryable = container.GetItemLinqQueryable<Post>(false);
            var idString = id.ToString();
            var post = queryable.FirstOrDefault(x => x.Id.ToString() == idString);
            return await Task.FromResult(post);
        }

        public async Task<IList<Post>> GetPostsAsync(GetAllPostsFilter filter = null, PaginationFilter paginationFilter = null)
        {
            var db = _client.GetDatabase(DATABASE);
            var container = db.GetContainer(CONTAINERID);
            var queryable = container.GetItemLinqQueryable<Post>(false);
            var list = queryable.ToList();
            return await Task.FromResult<IList<Post>>(list);
        }

        public async Task<bool> UpdatePostAsync(Post postToupdate)
        {
            var db = _client.GetDatabase(DATABASE);
            var container = db.GetContainer(CONTAINERID);
            try
            {
                var result = await container.UpsertItemAsync(postToupdate);
            }
            catch (CosmosException cex)
            {
                // log error here
                return false;
            }
            return true;
        }

        public async Task<bool> UserOwnsPost(Guid id, string userId)
        {
            var post = await GetPostbyIdAsync(id);

            if (post?.UserId == userId)
                return true;

            return false;
        }

        public Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Tag> GetTagByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateTagAsync(Tag newTag)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTagAsync(string tagName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTagAsync(Tag tag)
        {
            throw new NotImplementedException();
        }
    }
}
