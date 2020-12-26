using Chirp.Contracts.V1;
using Chirp.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Chirp.IntegrationTests
{
    public class PostsControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAll_NoPosts_ReturnsEmptyResponse()
        {
            await AuthenticateAsync();

            var response = await TestClient.GetAsync(ApiRoutes.Posts.Get);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Newtonsoft.Json.JsonConvert.DeserializeObject<List<Post>>(await response.Content.ReadAsStringAsync()).Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ReturnsPost_IfExists()
        {
            await AuthenticateAsync();
            var createdPost = await CreatePostAsync(new Contracts.V1.Requests.CreatePostRequest { Name = "test" });

            var response = await TestClient.GetAsync(ApiRoutes.Posts.GetById.Replace("{postId}", createdPost.Id.ToString()));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedPost = Newtonsoft.Json.JsonConvert.DeserializeObject<Post>(await response.Content.ReadAsStringAsync());
            returnedPost.Id.Should().Be(createdPost.Id);
            returnedPost.Name.Should().Be("test");
        }


    }
}
